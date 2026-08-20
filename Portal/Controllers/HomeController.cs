using BusinessLayer.Models;
using BusinessLayer.Services.ExportService;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.DatatableModels;
using System.Diagnostics;

namespace Portal.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ITeachersData _teachersData;
        private readonly IStudentsData _studentsData;
        private readonly IClassesData _classesData;
        private readonly ISubjectsData _subjectsData;
        private readonly IExportService _exportService;

        public HomeController(ITeachersData teachersData, IStudentsData studentsData, IClassesData classesData,
            ISubjectsData subjectsData, IExportService exportService)
        {
            _teachersData = teachersData;
            _studentsData = studentsData;
            _classesData = classesData;
            _subjectsData = subjectsData;
            _exportService = exportService;
        }

        public IActionResult Index()
        {
            var userName = User.Identity?.Name;
            ViewBag.UserName = userName;

            if (User.IsInRole("Admin"))
            {
                AdminDashboardViewModel model = new AdminDashboardViewModel();
                var teacher = _teachersData.GetAllTeachers();
                var student = _studentsData.GetAllStudents();
                var classes = _classesData.GetAllClasses();
                var subject = _subjectsData.GetAllSubjects();

                model.TotalTeacher = teacher.Count;
                model.TotalStudent = student.Count;
                model.TotalClass = classes.Count;
                model.TotalSubject = subject.Count;

                return View(model);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DtParameters? dtParameters)
        {
            if (dtParameters == null)
            {
                return BadRequest(new
                {
                    error = "DataTables parameters are null."
                });
            }

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

            var result = _classesData.GetAllClasses().AsQueryable();

            int totalResultsCount = result.Count();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                string search = searchBy.Trim();

                result = result.Where(r =>
                    (r.ClassName != null &&
                     r.ClassName.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.Section != null &&
                     r.Section.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.AcademicYear != null &&
                     r.AcademicYear.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    r.MaxCapacity
                        .ToString()
                        .Contains(search)

                    ||

                    r.IsActive
                        .ToString()
                        .Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
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

            return Json(new DtResult<Classes>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = data
            });
        }
    }
}
