using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Newtonsoft.Json;
using BusinessLayer.Services;

namespace Portal.Controllers
{
	public class ClassesController : BaseController
	{
		private readonly IClassesData _classesData;
		private readonly IExportService _exportService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public ClassesController (IClassesData classesData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
		{
			this._classesData = classesData;
			this._exportService = exportService;
			this._hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add()
		{
			var model = new ClassesViewModelAdd();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Insert(ClassesViewModelAdd model)
		{
            
            long classesId = _classesData.InsertClasses(new Classes()
            {
                ClassName = model.ClassName,
				Section = model.Section,
				AcademicYear = model.AcademicYear,
				MaxCapacity = model.MaxCapacity,
				ClassGuid = model.ClassGuid,
				CreatedBy = UserGuid,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "Classes");
		}
		public IActionResult Edit(int id)
		{
            ClassesViewModelEdit model = new ClassesViewModelEdit();
            var classes = _classesData.GetClassesById(id);
            if (classes != null)
            {
                model.Id = classes.Id;
				model.ClassName = classes.ClassName;
				model.Section = classes.Section;
				model.AcademicYear = classes.AcademicYear;
				model.MaxCapacity = classes.MaxCapacity;
				model.ClassGuid = classes.ClassGuid;
				model.IsActive = classes.IsActive;
            }
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> Update(ClassesViewModelEdit model)
		{
            
            _classesData.UpdateClassesById(new Classes()
            {
                Id = model.Id,
				ClassName = model.ClassName,
				Section = model.Section,
				AcademicYear = model.AcademicYear,
				MaxCapacity = model.MaxCapacity,
				ClassGuid = model.ClassGuid,
				IsActive = model.IsActive,
				ModifiedBy = UserGuid,
				ModifiedDate = DateTime.Now
            });
			return RedirectToAction("Index", "Classes");
		}
        //[HttpPost("Classes/LoadTable")]
        //public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        //{
        //    var searchBy = dtParameters.Search?.Value;

        //    // if we have an empty search then just order the results by Id ascending
        //    var orderCriteria = "Id";
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = _classesData.GetAllClasses().AsQueryable();
        //    var totalResultsCount = result.Count();
        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.ClassName != null && r.ClassName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.Section != null && r.Section.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.AcademicYear != null && r.AcademicYear.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.MaxCapacity != null && r.MaxCapacity.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ClassGuid != null && r.ClassGuid.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ModifiedBy != null && r.ModifiedBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
        //    var filteredResultsCount = result.Count();


        //    return Json(new DtResult<Classes>
        //    {
        //        Draw = dtParameters.Draw,
        //        RecordsTotal = totalResultsCount,
        //        RecordsFiltered = filteredResultsCount,
        //        Data = result
        //            .Skip(dtParameters.Start)
        //            .Take(dtParameters.Length)
        //            .ToList()
        //    });
        //}
        //[HttpPost("Classes/ExportTable")]
        //public async Task<IActionResult> ExportTable([FromQuery] string format, [FromForm] string dtParametersJson)
        //{
        //    var dtParameters = new DtParameters();
        //    if (!string.IsNullOrEmpty(dtParametersJson))
        //    {
        //        dtParameters = JsonConvert.DeserializeObject<DtParameters>(dtParametersJson);
        //    }

        //    var searchBy = dtParameters.Search?.Value;

        //    // if we have an empty search then just order the results by Id ascending
        //    var orderCriteria = "Id";
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = _classesData.GetAllClasses().AsQueryable();

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.ClassName != null && r.ClassName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.Section != null && r.Section.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.AcademicYear != null && r.AcademicYear.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.MaxCapacity != null && r.MaxCapacity.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ClassGuid != null && r.ClassGuid.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.ModifiedBy != null && r.ModifiedBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
        //                r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    var resultList = result.ToList();

        //    switch (format)
        //    {
        //        case ExportFormat.Excel:
        //            return File(
        //                await _exportService.ExportToExcel(resultList),
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "data.xlsx");

        //        case ExportFormat.Csv:
        //            return File(_exportService.ExportToCsv(resultList),
        //                "application/csv",
        //                "data.csv");

        //        case ExportFormat.Html:
        //            return File(_exportService.ExportToHtml(resultList),
        //                "application/csv",
        //                "data.html");
        //    }

        //    return null;
        //}
    }
}
