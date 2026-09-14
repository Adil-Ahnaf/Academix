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

        public IActionResult MarkSubmissions(Guid assignmentGuid)
        {
            var model = new MarkSubmissionsViewModel();

            model.AssignmentInfo = _assignmentsData.GetAssignmentByAssignmentGuid(assignmentGuid);
            model.ClassInfo = _classesData.GetClassesById(model.AssignmentInfo.ClassId);

            return View(model);
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

        public IActionResult LoadTable([FromBody] DtParameters? dtParameters)
        {
            if (dtParameters == null)
            {
                return BadRequest(new
                {
                    error = "DataTables parameters are null."
                });
            }

            Guid? assignmentGuid = dtParameters?.AssignmentGuid;
            string? searchBy = dtParameters.Search?.Value;
            string orderCriteria = "Id";
            bool orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0 && dtParameters.Columns != null)
            {
                int columnIndex = dtParameters.Order[0].Column;

                if (columnIndex >= 0 && columnIndex < dtParameters.Columns.Length)
                {
                    string? requestedColumn = dtParameters.Columns[columnIndex].Data;

                    if (!string.IsNullOrWhiteSpace(requestedColumn))
                    {
                        orderCriteria = requestedColumn;
                    }
                }
                orderAscendingDirection = !string.Equals(dtParameters.Order[0].Dir, "desc", StringComparison.OrdinalIgnoreCase);
            }

            var result = _submissionsData.GetAllSubmissionsByAssignmentGuid(assignmentGuid.Value).AsQueryable();

            int totalResultsCount = result.Count();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                string search = searchBy.Trim();

                result = result.Where(r =>
                    (r.StudentCode != null &&
                     r.StudentCode.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.FullName != null &&
                     r.FullName.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.FileName != null &&
                     r.FileName.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    || 
                    
                    (r.Marks != null &&
                     r.Marks.ToString().Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))
                    ||

                    (r.Feedback != null &&
                     r.Feedback.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))
                );
            }

            result = orderAscendingDirection
                ? result.OrderByDynamic(
                    orderCriteria,
                    DtOrderDir.Asc)

                : result.OrderByDynamic(
                    orderCriteria,
                    DtOrderDir.Desc);

            int filteredResultsCount = result.Count();

            int start = Math.Max(
                dtParameters.Start,
                0);

            int length = dtParameters.Length;


            if (length <= 0)
            {
                length = 10;
            }


            var data = result
                .Skip(start)
                .Take(length)
                .ToList();

            return Json(new DtResult<Submissions>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = data
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
