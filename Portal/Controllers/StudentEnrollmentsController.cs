using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Newtonsoft.Json;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize]
    public class StudentEnrollmentsController : BaseController
    {
        private readonly IStudentEnrollmentsData _studentEnrollmentsData;
        private readonly IExportService _exportService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StudentEnrollmentsController(IStudentEnrollmentsData studentEnrollmentsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
        {
            this._studentEnrollmentsData = studentEnrollmentsData;
            this._exportService = exportService;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new StudentEnrollmentsViewModelAdd();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(StudentEnrollmentsViewModelAdd model)
        {

            long studentEnrollmentsId = _studentEnrollmentsData.InsertStudentEnrollments(new StudentEnrollments()
            {
                StudentId = model.StudentId,
                ClassId = model.ClassId,
                CreatedDate = DateTime.Now,
                IsActive = true
            });
            return RedirectToAction("Index", "StudentEnrollments");
        }
        public IActionResult Edit(int id)
        {
            StudentEnrollmentsViewModelEdit model = new StudentEnrollmentsViewModelEdit();
            var studentEnrollments = _studentEnrollmentsData.GetStudentEnrollmentsById(id);
            if (studentEnrollments != null)
            {
                model.Id = studentEnrollments.Id;
                model.StudentId = studentEnrollments.StudentId;
                model.ClassId = studentEnrollments.ClassId;
                model.IsActive = studentEnrollments.IsActive;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(StudentEnrollmentsViewModelEdit model)
        {

            _studentEnrollmentsData.UpdateStudentEnrollmentsById(new StudentEnrollments()
            {
                Id = model.Id,
                StudentId = model.StudentId,
                ClassId = model.ClassId,
                IsActive = model.IsActive,
                ModifiedDate = DateTime.Now
            });
            return RedirectToAction("Index", "StudentEnrollments");
        }
        [HttpPost("StudentEnrollments/LoadTable")]
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

            var result = _studentEnrollmentsData.GetAllStudentEnrollments().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.StudentId != null && r.StudentId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ClassId != null && r.ClassId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<StudentEnrollments>
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
        [HttpPost("StudentEnrollments/ExportTable")]
        public async Task<IActionResult> ExportTable([FromQuery] string format, [FromForm] string dtParametersJson)
        {
            var dtParameters = new DtParameters();
            if (!string.IsNullOrEmpty(dtParametersJson))
            {
                dtParameters = JsonConvert.DeserializeObject<DtParameters>(dtParametersJson);
            }

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

            var result = _studentEnrollmentsData.GetAllStudentEnrollments().AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.StudentId != null && r.StudentId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ClassId != null && r.ClassId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            var resultList = result.ToList();

            switch (format)
            {
                case ExportFormat.Excel:
                    return File(
                        await _exportService.ExportToExcel(resultList),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "data.xlsx");

                case ExportFormat.Csv:
                    return File(_exportService.ExportToCsv(resultList),
                        "application/csv",
                        "data.csv");

                case ExportFormat.Html:
                    return File(_exportService.ExportToHtml(resultList),
                        "application/csv",
                        "data.html");
            }

            return null;
        }
    }
}
