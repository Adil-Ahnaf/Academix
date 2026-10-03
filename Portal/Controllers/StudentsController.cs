using BusinessLayer.Models;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
    [Authorize]
    public class StudentsController : BaseController
    {
        private readonly IStudentsData _studentsData;
        private readonly IClassesData _classesData;

        public StudentsController(IStudentsData studentsData, IClassesData classesData)
        {
            _studentsData = studentsData;
            _classesData = classesData;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/StudentDashboard")]
        public IActionResult StudentDashboard()
        {
            var student = _studentsData.GetStudentByAspNetUserId(UserGuid);
            if (student == null)
            {
                return NotFound();
            }

            StudentDashboardViewModel model = new StudentDashboardViewModel
            {
                Student = student,
                AllClasses = _classesData.GetEnrolledClassesByStudentGuid(student.StudentGuid)
            };
            return View(model);
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

            var result = _studentsData.GetAllStudents().AsQueryable();

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

                    (r.Gender != null &&
                     r.Gender.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

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

            return Json(new DtResult<Students>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = data
            });
        }
    }
}
