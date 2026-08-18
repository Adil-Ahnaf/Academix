using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
	public class TeacherEnrollmentsController : BaseController
	{
		private readonly ITeacherEnrollmentsData _teacherEnrollmentsData;
		private readonly IExportService _exportService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public TeacherEnrollmentsController (ITeacherEnrollmentsData teacherEnrollmentsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
		{
			this._teacherEnrollmentsData = teacherEnrollmentsData;
			this._exportService = exportService;
			this._hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add()
		{
			var model = new TeacherEnrollmentsViewModelAdd();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Insert(TeacherEnrollmentsViewModelAdd model)
		{
            
            long teacherEnrollmentsId = _teacherEnrollmentsData.InsertTeacherEnrollments(new TeacherEnrollments()
            {
                TeacherId = model.TeacherId,
				ClassId = model.ClassId,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "TeacherEnrollments");
		}
		public IActionResult Edit(int id)
		{
            TeacherEnrollmentsViewModelEdit model = new TeacherEnrollmentsViewModelEdit();
            var teacherEnrollments = _teacherEnrollmentsData.GetTeacherEnrollmentsById(id);
            if (teacherEnrollments != null)
            {
                model.Id = teacherEnrollments.Id;
				model.TeacherId = teacherEnrollments.TeacherId;
				model.ClassId = teacherEnrollments.ClassId;
				model.IsActive = teacherEnrollments.IsActive;
            }
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> Update(TeacherEnrollmentsViewModelEdit model)
		{
            
            _teacherEnrollmentsData.UpdateTeacherEnrollmentsById(new TeacherEnrollments()
            {
                Id = model.Id,
				TeacherId = model.TeacherId,
				ClassId = model.ClassId,
				IsActive = model.IsActive,
				ModifiedDate = DateTime.Now
            });
			return RedirectToAction("Index", "TeacherEnrollments");
		}
        [HttpPost("TeacherEnrollments/LoadTable")]
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

            var result = _teacherEnrollmentsData.GetAllTeacherEnrollments().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.TeacherId != null && r.TeacherId.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ClassId != null && r.ClassId.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<TeacherEnrollments>
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
