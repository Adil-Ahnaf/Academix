using BusinessLayer.Models;
using BusinessLayer.Services.ExportService;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Portal.Extensions;
using Portal.Helpers;
using Portal.Models;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
    [Authorize]
    public class AssignmentsController : BaseController
    {
        private readonly IAssignmentsData _assignmentsData;
        private readonly IClassesData _classesData;
        private readonly IStudentsData _studentsData;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailHelper _emailHelper;
        private readonly IConfiguration _configuration;
        public AssignmentsController(IAssignmentsData assignmentsData, IClassesData classesData, IStudentsData studentsData,
            IWebHostEnvironment hostingEnvironment, IEmailHelper emailHelper, IConfiguration configuration)
        {
            _assignmentsData = assignmentsData;
            _classesData = classesData;
            _studentsData = studentsData;
            _hostingEnvironment = hostingEnvironment;
            _emailHelper = emailHelper;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Assignments/All/{classGuid}")]
        public IActionResult AllAssignments(Guid classGuid)
        {
            var model = new AssignmentsViewModel();

            model.ClassInfo = _classesData.GetClassesByClassGuid(classGuid);
            model.AllAssignment = _assignmentsData.GetAllAssignmentByClassGuid(classGuid);
            model.TotalAssignment = model.AllAssignment.Count;
            model.PublishedAssignment = model.AllAssignment.Count(a => a.IsPublish);
            model.DraftAssignment = model.AllAssignment.Count(a => !a.IsPublish);
            model.TotalSubmissions = model.AllAssignment.Sum(a => a.TotalSubmissions);
            model.TotalStudents = _studentsData.GetEnrolledStudentsByClassGuid(classGuid).Count;

            return View(model);
        }

        [HttpGet("Assignments/Add/{classGuid}")]
        public IActionResult Add(Guid classGuid)
        {
            var model = new AssignmentsViewModelAdd
            {
                ClassInfo = _classesData.GetClassesByClassGuid(classGuid)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(AssignmentsViewModelAdd model)
        {
            string? filePath = null;

            if (model.FilePath != null && model.FilePath.Length > 0)
            {
                string folderName = $"{model.ClassName}_{model.Section}";
                string pathToSave = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                filePath = Path.Combine(pathToSave, model.FilePath.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FilePath.CopyToAsync(fileStream);
                }
            }

            long assignmentsId = _assignmentsData.InsertAssignments(new Assignments()
            {
                ClassId = model.ClassId,
                Title = model.Title,
                Description = model.Description,
                FilePath = filePath,
                Marks = model.Marks,
                Deadline = model.Deadline,
                IsPublish = model.IsPublish,
                CreatedDate = DateTime.Now,
                IsActive = true
            });

            // Send notification to students in the class about the new assignment
            if (model.IsPublish)
            {
                // Implementation for sending notification
                var students = _studentsData.GetEnrolledStudentsEmailByClassGuid(model.ClassGuid).Select(s => s.EmailAddress).ToList();

                string template = string.Empty;
                using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "NewAssignmentAddNotifyEmail.html")))
                {
                    template = await reader.ReadToEndAsync();
                }

                var emailBody = template.Replace("{AssignmentTitle}", model.Title)
                                        .Replace("{ClassName}", model.ClassName)
                                        .Replace("{SectionName}", model.Section)
                                        .Replace("{TotalMarks}", model.Marks.ToString())
                                        .Replace("{Deadline}", model.Deadline.ToString("dd MMM yyyy"));

                await _emailHelper.SendBulkEmailAsync(students, "New Assignment Added", emailBody);
            }

            return RedirectToAction("AllAssignments", "Assignments", new { classGuid = model.ClassGuid });
        }

        [HttpGet("Assignments/Edit/{assignmentGuid}")]
        public IActionResult Edit(Guid assignmentGuid)
        {
            AssignmentsViewModelEdit model = new AssignmentsViewModelEdit();
            var assignments = _assignmentsData.GetAssignmentByAssignmentGuid(assignmentGuid);
            if (assignments != null)
            {
                model.Title = assignments.Title;
                model.ExistingFilePath = assignments.FilePath;
                model.Description = assignments.Description;
                model.Marks = assignments.Marks;
                model.Deadline = assignments.Deadline;
                model.IsPublish = assignments.IsPublish;
                model.AssignmentGuid = assignments.AssignmentGuid;
                model.ClassInfo = _classesData.GetClassesById(assignments.ClassId);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AssignmentsViewModelEdit model)
        {
            var existingAssignment = _assignmentsData.GetAssignmentByAssignmentGuid(model.AssignmentGuid);

            if (existingAssignment == null)
            {
                return NotFound();
            }

            string? filePath = existingAssignment.FilePath;

            if (model.FilePath != null && model.FilePath.Length > 0)
            {
                // Save the new file
                string folderName = $"{model.ClassName}_{model.Section}";
                string pathToSave = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                filePath = Path.Combine(pathToSave, model.FilePath.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FilePath.CopyToAsync(fileStream);
                }

                // Delete previous attachment
                if (!string.IsNullOrEmpty(existingAssignment.FilePath) && System.IO.File.Exists(existingAssignment.FilePath))
                {
                    System.IO.File.Delete(existingAssignment.FilePath);
                }

                filePath = Path.Combine(pathToSave, model.FilePath.FileName);
            }

            // update the assignment with the new values
            _assignmentsData.UpdateAssignmentsById(new Assignments()
            {
                Id = existingAssignment.Id,
                Title = model.Title,
                Description = model.Description,
                FilePath = filePath,
                Marks = model.Marks,
                Deadline = model.Deadline,
                IsPublish = model.IsPublish,
                ModifiedDate = DateTime.Now
            });

            return RedirectToAction("AllAssignments", "Assignments", new { classGuid = model.ClassGuid });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid assignmentGuid, Guid classGuid)
        {
            var existingAssignment = _assignmentsData.GetAssignmentByAssignmentGuid(assignmentGuid);
            if (existingAssignment == null)
            {
                return NotFound();
            }

            // Delete the assignment
            _assignmentsData.DeleteAssignmentsById(existingAssignment.Id);

            // Delete the attachment file if it exists
            if (!string.IsNullOrEmpty(existingAssignment.FilePath) && System.IO.File.Exists(existingAssignment.FilePath))
            {
                System.IO.File.Delete(existingAssignment.FilePath);
            }

            return RedirectToAction("AllAssignments", "Assignments", new { classGuid = classGuid });
        }

        [HttpPost("Assignments/LoadTable")]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = _assignmentsData.GetAllAssignments().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.ClassId != null && r.ClassId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Title != null && r.Title.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Description != null && r.Description.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Marks != null && r.Marks.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Deadline != null && r.Deadline.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsPublish != null && r.IsPublish.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.AssignmentGuid != null && r.AssignmentGuid.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Assignments>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        public IActionResult Download(Guid assignmentGuid)
        {
            var assignment = _assignmentsData.GetAssignmentByAssignmentGuid(assignmentGuid);

            if (assignment == null)
                return NotFound();

            var filePath = assignment.FilePath;

            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileName = Path.GetFileName(filePath);

            // Open file in browser when supported
            Response.Headers.Append(
                "Content-Disposition",
                $"inline; filename=\"{fileName}\""
            );

            return PhysicalFile(
                filePath,
                contentType
            );
        }

    }
}
