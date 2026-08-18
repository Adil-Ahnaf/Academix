using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
	public class SubjectsController : BaseController
	{
		private readonly ISubjectsData _subjectsData;
		private readonly IExportService _exportService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public SubjectsController (ISubjectsData subjectsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
		{
			this._subjectsData = subjectsData;
			this._exportService = exportService;
			this._hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add()
		{
			var model = new SubjectsViewModelAdd();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Insert(SubjectsViewModelAdd model)
		{
            
            long subjectsId = _subjectsData.InsertSubjects(new Subjects()
            {
                Name = model.Name,
				CreatedBy = UserGuid,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "Subjects");
		}
		public IActionResult Edit(int id)
		{
            SubjectsViewModelEdit model = new SubjectsViewModelEdit();
            var subjects = _subjectsData.GetSubjectsById(id);
            if (subjects != null)
            {
                model.Id = subjects.Id;
				model.Name = subjects.Name;
				model.IsActive = subjects.IsActive;
            }
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> Update(SubjectsViewModelEdit model)
		{
            
            _subjectsData.UpdateSubjectsById(new Subjects()
            {
                Id = model.Id,
				Name = model.Name,
				IsActive = model.IsActive,
				ModifiedBy = UserGuid,
				ModifiedDate = DateTime.Now
            });
			return RedirectToAction("Index", "Subjects");
		}
        [HttpPost("Subjects/LoadTable")]
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

            var result = _subjectsData.GetAllSubjects().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedBy != null && r.ModifiedBy.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Subjects>
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
