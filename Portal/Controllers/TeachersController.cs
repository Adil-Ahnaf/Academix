using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize]
    public class TeachersController : BaseController
    {
        private readonly ITeachersData _teachersData;
        private readonly IClassesData _classesData;

        public TeachersController(ITeachersData teachersData, IClassesData classesData)
        {
            _teachersData = teachersData;
            _classesData = classesData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TeacherDashboard()
        {
            var teacher = _teachersData.GetTeacherByAspNetUserId(UserGuid);
            if (teacher == null)
            {
                return NotFound();
            }
            TeacherDashboardViewModel model = new TeacherDashboardViewModel
            {
                Teacher = teacher,
                AllClasses = _classesData.GetEnrolledClassesByTeacherGuid(teacher.TeacherGuid)
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

            var result = _teachersData.GetAllTeachers().AsQueryable();

            int totalResultsCount = result.Count();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                string search = searchBy.Trim();

                result = result.Where(r =>
                    (r.FullName != null &&
                     r.FullName.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.Department != null &&
                     r.Department.Contains(
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

            return Json(new DtResult<Teachers>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = data
            });
        }
    }
}
