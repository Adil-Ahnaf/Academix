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

        [HttpGet("Enrollments/{teacherGuid:guid}")]
        public IActionResult Index(Guid teacherGuid)
        {
            var model = new TeacherEnrollmentsViewModel
            {
                Teacher = _teachersData.GetTeachersById(teacherGuid),
                TeacherEnrollmentsList = _teacherEnrollmentsData.GetATeacherAllEnrollments(teacherGuid),
                ClassesList = _classesData.GetAllActiveClassesForATeacher(teacherGuid)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Guid classGuid, Guid teacherGuid)
        {
            var class_data = _classesData.GetClassesById(classGuid);
            var teacher_data = _teachersData.GetTeachersById(teacherGuid);

            long teacherEnrollmentsId = _teacherEnrollmentsData.InsertTeacherEnrollments(new TeacherEnrollments()
            {
                TeacherId = teacher_data.Id,
                ClassId = class_data.Id,
                CreatedDate = DateTime.Now,
                IsActive = true
            });

            return Json(new
            {
                success = true,
                academicYear = class_data.AcademicYear,
                className = class_data.ClassName,
                subject = class_data.SubjectName,
                section = class_data.Section,
                maxCapacity = class_data.MaxCapacity
            });
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
