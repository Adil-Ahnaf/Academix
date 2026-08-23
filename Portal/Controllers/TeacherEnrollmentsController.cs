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
        private readonly ITeachersData _teachersData;
        private readonly IClassesData _classesData;
        private readonly IExportService _exportService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TeacherEnrollmentsController(ITeacherEnrollmentsData teacherEnrollmentsData, ITeachersData teachersData,
            IClassesData classesData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
        {
            _teacherEnrollmentsData = teacherEnrollmentsData;
            _teachersData = teachersData;
            _classesData = classesData;
            _exportService = exportService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("Enrollments/{teacherGuid?}")]
        public IActionResult Index(Guid? teacherGuid)
        {
            var model = new TeacherEnrollmentsViewModel();
            if (teacherGuid.HasValue)
            {
                model.Teacher = _teachersData.GetTeachersById(teacherGuid.Value);
                model.TeacherEnrollmentsList = _teacherEnrollmentsData.GetATeacherAllEnrollments(teacherGuid.Value);
            }
            model.ClassesList = _classesData.GetAllActiveClasses();
            return View(model);
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
    }
}
