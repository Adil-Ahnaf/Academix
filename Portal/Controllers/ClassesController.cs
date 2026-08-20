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
				ClassGuid = Guid.NewGuid(),
				CreatedBy = UserGuid,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "Home");
		}
        [HttpGet("Classes/Edit/{classGuid:guid}")]
        public IActionResult Edit(Guid classGuid)
		{
            ClassesViewModelEdit model = new ClassesViewModelEdit();
            var classes = _classesData.GetClassesById(classGuid);
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
			return RedirectToAction("Index", "Home");
		}
    }
}
