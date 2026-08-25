using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize]
    public class TeacherEnrollmentsController : BaseController
    {
        private readonly ITeacherEnrollmentsData _teacherEnrollmentsData;
        private readonly ITeachersData _teachersData;
        private readonly IClassesData _classesData;
        
        public TeacherEnrollmentsController(ITeacherEnrollmentsData teacherEnrollmentsData,
            ITeachersData teachersData, IClassesData classesData)
        {
            _teacherEnrollmentsData = teacherEnrollmentsData;
            _teachersData = teachersData;
            _classesData = classesData;
        }
        
        [HttpGet("Enrollments/{teacherGuid:guid}")]
        public IActionResult Index(Guid teacherGuid)
        {
            var model = new TeacherEnrollmentsViewModel
            {
                Teacher = _teachersData.GetTeachersById(teacherGuid),
                TeacherEnrollmentsList = _teacherEnrollmentsData.GetATeacherAllEnrollments(teacherGuid),
                ClassesList = _classesData.GetAllAvailableClassesForATeacher()
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
        
        [HttpPost]
        public async Task<IActionResult> Delete(Guid classGuid, Guid teacherGuid)
        {
            var class_data = _classesData.GetClassesById(classGuid);
            var teacher_data = _teachersData.GetTeachersById(teacherGuid);

            var enrollment = _teacherEnrollmentsData.GetTeacherEnrollmentByClassAndTeacher(classGuid, teacherGuid);
            if (enrollment == null)
            {
                return Json(new { success = false, message = "Enrollment not found." });
            }

            _teacherEnrollmentsData.DeleteTeacherEnrollmentsById(enrollment.Id);

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
    }
}
