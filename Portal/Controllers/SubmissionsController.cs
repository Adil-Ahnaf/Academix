using BusinessLayer.Models;
using BusinessLayer.Services.ExportService;
using DataAccessLayer.DataAccess;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
    [Authorize]
    public class SubmissionsController : BaseController
    {
        private readonly ISubmissionsData _submissionsData;
        private readonly IAssignmentsData _assignmentsData;
        private readonly IClassesData _classesData;
        private readonly IStudentsData _studentsData;
        private readonly IExportService _exportService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SubmissionsController(ISubmissionsData submissionsData, IAssignmentsData assignmentsData, IClassesData classesData,
            IStudentsData studentsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
        {
            this._submissionsData = submissionsData;
            this._assignmentsData = assignmentsData;
            this._classesData = classesData;
            this._studentsData = studentsData;
            this._exportService = exportService;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Guid assignmentGuid, IFormFile submissionFile)
        {
            var assignment = _assignmentsData.GetAssignmentByAssignmentGuid(assignmentGuid);

            if (assignment == null)
            {
                return NotFound();
            }

            var classInfo = _classesData.GetClassesById(assignment.ClassId);
            var studentInfo = _studentsData.GetStudentByAspNetUserId(UserGuid);

            // Folder names using GUIDs
            string classFolder = classInfo.ClassGuid.ToString();
            string assignmentFolder = assignment.AssignmentGuid.ToString();

            // Physical folder path
            string submissionFolder = Path.Combine(
                _hostingEnvironment.WebRootPath,
                "submissions",
                classFolder,
                assignmentFolder
            );

            // Create directory if it doesn't exist
            if (!Directory.Exists(submissionFolder))
            {
                Directory.CreateDirectory(submissionFolder);
            }

            // Keep original file extension
            string extension = Path.GetExtension(submissionFile.FileName);

            // File name
            string fileName = $"{studentInfo.StudentCode}_Assignment{extension}";

            // Physical file path
            string physicalFilePath = Path.Combine(submissionFolder, fileName);

            // Save file
            using (var stream = new FileStream(physicalFilePath, FileMode.Create))
            {
                await submissionFile.CopyToAsync(stream);
            }

            // Web-relative path stored in database
            string filePath = $"/submissions/{classFolder}/{assignmentFolder}/{fileName}";

            // Insert submission
            _submissionsData.InsertSubmissions(new Submissions
            {
                AssignmentId = assignment.Id,
                StudentId = studentInfo.Id,
                FileName = fileName,
                FilePath = filePath,
                SubmissionGuid = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = UserGuid,
                IsActive = true
            });

            return RedirectToAction("AllAssignments", "Assignments", new { classGuid = classInfo.ClassGuid });
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid submissionGuid, IFormFile submissionFile)
        {
            var submission = _submissionsData.GetSubmissionBySubmissionGuid(submissionGuid);

            if (submission == null)
            {
                return NotFound();
            }

            var assignment = _assignmentsData.GetAssignmentsById(submission.AssignmentId);
            var classInfo = _classesData.GetClassesById(assignment.ClassId);

            // Get existing physical file path from database
            string existingFilePath = Path.Combine(
                _hostingEnvironment.WebRootPath,
                submission.FilePath.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            // Retain the existing file name (without extension)
            string oldFileName = submission.FileName;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(oldFileName);

            string newExtension = Path.GetExtension(submissionFile.FileName);
            string newFileName = fileNameWithoutExtension + newExtension;

            // Remove existing file
            if (System.IO.File.Exists(existingFilePath))
            {
                System.IO.File.Delete(existingFilePath);
            }

            // Get the existing folder
            string submissionFolder = Path.GetDirectoryName(existingFilePath);

            // Make sure folder exists
            if (!Directory.Exists(submissionFolder))
            {
                Directory.CreateDirectory(submissionFolder);
            }

            // Get new file extension
            string extension = Path.GetExtension(submissionFile.FileName);

            // New physical file path
            string newPhysicalPath = Path.Combine(submissionFolder, newFileName);

            // Save new file
            using (var stream = new FileStream(newPhysicalPath, FileMode.Create))
            {
                await submissionFile.CopyToAsync(stream);
            }

            // Update database if only the file name/extension changes
            string newFilePath = submission.FilePath;

            int lastSlash = newFilePath.LastIndexOf('/');

            if (lastSlash >= 0)
            {
                newFilePath = newFilePath.Substring(0, lastSlash + 1) + newFileName;
            }

            _submissionsData.UpdateSubmissionsBySubmissionGuid(new Submissions
            {
                SubmissionGuid = submissionGuid,
                FileName = newFileName,
                FilePath = newFilePath,
                ModifiedDate = DateTime.Now,
                ModifiedBy = UserGuid
            });

            TempData["Success"] = "Assignment file updated successfully.";

            return RedirectToAction("AllAssignments", "Assignments", new { classGuid = classInfo.ClassGuid });
        }

        [HttpPost("Submissions/LoadTable")]
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

            var result = _submissionsData.GetAllSubmissions().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.AssignmentId != null && r.AssignmentId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.StudentId != null && r.StudentId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.FileName != null && r.FileName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.FilePath != null && r.FilePath.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Marks != null && r.Marks.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Feedback != null && r.Feedback.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.SubmissionGuid != null && r.SubmissionGuid.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ModifiedBy != null && r.ModifiedBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Submissions>
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

        public IActionResult ViewSubmissionFile(Guid submissionGuid)
        {
            var submission = _submissionsData.GetSubmissionBySubmissionGuid(submissionGuid);

            if (submission == null)
            {
                return NotFound();
            }

            // Convert web-relative path to physical path
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, submission.FilePath
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
