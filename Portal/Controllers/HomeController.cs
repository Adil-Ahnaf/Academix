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

        [HttpPost("Home/Index/LoadTable")]
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

            var result = _classesData.GetAllClasses().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.ClassName != null && r.ClassName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Section != null && r.Section.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.AcademicYear != null && r.AcademicYear.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.MaxCapacity != null && r.MaxCapacity.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ClassGuid != null && r.ClassGuid.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Classes>
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
    }
}
