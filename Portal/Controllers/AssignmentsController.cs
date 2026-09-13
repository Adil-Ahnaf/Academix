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
        private readonly ISubmissionsData _submissionsData;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailHelper _emailHelper;
        private readonly IConfiguration _configuration;
        public AssignmentsController(IAssignmentsData assignmentsData, IClassesData classesData, IStudentsData studentsData,
            ISubmissionsData submissionsData, IWebHostEnvironment hostingEnvironment, IEmailHelper emailHelper, IConfiguration configuration)
        {
            _assignmentsData = assignmentsData;
            _classesData = classesData;
            _studentsData = studentsData;
            _submissionsData = submissionsData;
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
            model.TotalStudents = _studentsData.GetEnrolledStudentsByClassGuid(classGuid).Count;

            if (User.IsInRole("Teacher"))
            {
                model.AllAssignment = _assignmentsData.GetAllAssignmentByClassGuid(classGuid);
                model.TotalAssignment = model.AllAssignment.Count;
                model.PublishedAssignment = model.AllAssignment.Count(a => a.IsPublish);
                model.DraftAssignment = model.AllAssignment.Count(a => !a.IsPublish);
                model.TotalSubmissions = model.AllAssignment.Sum(a => a.TotalSubmissions);
            }
            else
            {
                // For students, only show published assignments
                model.AllAssignment = _assignmentsData.GetAllAssignmentByClassGuid(classGuid).Where(a => a.IsPublish).ToList();
                model.AllSubmissions = _submissionsData.GetSubmissionsByStudentAspNetUserId(UserGuid);
            }

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

            // Generate AssignmentGuid before saving the file
            Guid assignmentGuid = Guid.NewGuid();

            if (model.FilePath != null && model.FilePath.Length > 0)
            {
                string classFolder = model.ClassGuid.ToString();
                string assignmentFolder = assignmentGuid.ToString();

                string pathToSave = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    "attachments",
                    classFolder,
                    assignmentFolder
                );

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                // Keep original file extension
                string extension = Path.GetExtension(model.FilePath.FileName);
                string fileName = Path.GetFileNameWithoutExtension(model.FilePath.FileName) + extension;

                // Physical path
                string physicalFilePath = Path.Combine(pathToSave, fileName);

                // Save file
                using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    await model.FilePath.CopyToAsync(fileStream);
                }

                // Store web-relative path in database
                filePath = $"/attachments/{classFolder}/{assignmentFolder}/{fileName}";
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
                AssignmentGuid = assignmentGuid,
                CreatedDate = DateTime.Now,
                IsActive = true
            });

            // Send notification to students when assignment is published
            if (model.IsPublish)
            {
                var students = _studentsData.GetEnrolledStudentsEmailByClassGuid(model.ClassGuid).Select(s => s.EmailAddress).ToList();

                string template = string.Empty;

                using (StreamReader reader = new StreamReader(Path.Combine(
                        _hostingEnvironment.WebRootPath,
                        "EmailTemplates",
                        "NewAssignmentAddNotifyEmail.html")))
                {
                    template = await reader.ReadToEndAsync();
                }

                var emailBody = template
                    .Replace("{AssignmentTitle}", model.Title)
                    .Replace("{ClassName}", model.ClassName)
                    .Replace("{SectionName}", model.Section)
                    .Replace("{TotalMarks}", Convert.ToString(model.Marks))
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
                string classFolder = model.ClassGuid.ToString();
                string assignmentFolder = model.AssignmentGuid.ToString();

                string pathToSave = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    "attachments",
                    classFolder,
                    assignmentFolder
                );

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                // Keep original extension
                string extension = Path.GetExtension(model.FilePath.FileName);

                string fileName = Path.GetFileNameWithoutExtension(model.FilePath.FileName) + extension;

                // New physical file path
                string newPhysicalFilePath = Path.Combine(pathToSave, fileName);

                // Save new file
                using (var fileStream = new FileStream(newPhysicalFilePath, FileMode.Create))
                {
                    await model.FilePath.CopyToAsync(fileStream);
                }

                // Delete previous attachment
                if (!string.IsNullOrEmpty(existingAssignment.FilePath))
                {
                    string oldPhysicalFilePath = Path.Combine(
                        _hostingEnvironment.WebRootPath,
                        existingAssignment.FilePath
                            .TrimStart('/', '\\')
                            .Replace(
                                "/",
                                Path.DirectorySeparatorChar.ToString()
                            )
                    );

                    if (System.IO.File.Exists(oldPhysicalFilePath))
                    {
                        System.IO.File.Delete(oldPhysicalFilePath);
                    }
                }

                // Store web-relative path in database
                filePath = $"/attachments/{classFolder}/{assignmentFolder}/{fileName}";
            }

            // Update assignment
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

            // Send notification when draft becomes published
            if (model.IsPublish && !existingAssignment.IsPublish)
            {
                var students = _studentsData.GetEnrolledStudentsEmailByClassGuid(model.ClassGuid).Select(s => s.EmailAddress).ToList();

                string template = string.Empty;

                using (StreamReader reader = new StreamReader(Path.Combine(
                        _hostingEnvironment.WebRootPath,
                        "EmailTemplates",
                        "NewAssignmentAddNotifyEmail.html")))
                {
                    template = await reader.ReadToEndAsync();
                }

                var emailBody = template
                    .Replace("{AssignmentTitle}", model.Title)
                    .Replace("{ClassName}", model.ClassName)
                    .Replace("{SectionName}", model.Section)
                    .Replace("{TotalMarks}", Convert.ToString(model.Marks))
                    .Replace("{Deadline}", model.Deadline.ToString("dd MMM yyyy"));

                await _emailHelper.SendBulkEmailAsync(students, "New Assignment Added", emailBody);
            }

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
            {
                return NotFound();
            }

            // Convert web-relative path to physical path
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, assignment.FilePath
                    .TrimStart('/', '\\')
                    .Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileName = Path.GetFileName(filePath);

            // Open file in browser when supported
            Response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileName}\"");

            return PhysicalFile(filePath, contentType);
        }
    }
}
