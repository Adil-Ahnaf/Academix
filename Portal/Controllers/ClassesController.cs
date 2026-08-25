using BusinessLayer.Models;
using BusinessLayer.Services;
using BusinessLayer.Services.ExportService;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
    [Authorize]
    public class ClassesController : BaseController
    {
        private readonly IClassesData _classesData;
        private readonly ISubjectsData _subjectsData;

        public ClassesController(IClassesData classesData, ISubjectsData subjectsData)
        {
            _classesData = classesData;
            _subjectsData = subjectsData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new ClassesViewModelAdd();
            model.SubjectOptions = GetSubjectList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ClassesViewModelAdd model)
        {

            long classesId = _classesData.InsertClasses(new Classes()
            {
                ClassName = model.ClassName,
                SubjectId = model.SubjectId,
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
                model.SubjectId = classes.SubjectId;
                model.Section = classes.Section;
                model.AcademicYear = classes.AcademicYear;
                model.MaxCapacity = classes.MaxCapacity;
                model.ClassGuid = classes.ClassGuid;
                model.IsActive = classes.IsActive;
                model.SubjectOptions = GetSubjectList();
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
                SubjectId = model.SubjectId,
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
        private SelectList GetSubjectList()
        {
            var subjectList = _subjectsData.GetAllSubjects();
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "Select", Value = "" });
            foreach (var item in subjectList)
            {
                selectListItems.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var selectList = new SelectList(selectListItems, "Value", "Text", 1);
            return selectList;
        }
    }
}
