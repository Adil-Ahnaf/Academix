using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Newtonsoft.Json;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize]
    public class StudentEnrollmentsController : BaseController
    {
        private readonly IStudentEnrollmentsData _studentEnrollmentsData;
        private readonly IStudentsData _studentsData;
        private readonly IClassesData _classesData;

        public StudentEnrollmentsController(IStudentEnrollmentsData studentEnrollmentsData, IStudentsData studentsData, IClassesData classesData)
        {
            _studentEnrollmentsData = studentEnrollmentsData;
            _studentsData = studentsData;
            _classesData = classesData;
        }

        [HttpGet("StudentEnrollments/{studentGuid:guid}")]
        public IActionResult Index(Guid studentGuid)
        {
            var model = new StudentEnrollmentsViewModel
            {
                Student = _studentsData.GetStudentsById(studentGuid),
                StudentEnrollmentsList = _studentEnrollmentsData.GetAStudentAllEnrollments(studentGuid),
                ClassesList = _classesData.GetAllAvailableClassesForAStudent(studentGuid)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Guid classGuid, Guid studentGuid)
        {
            var class_data = _classesData.GetClassesById(classGuid);
            var student_data = _studentsData.GetStudentsById(studentGuid);

            long studentEnrollmentsId = _studentEnrollmentsData.InsertStudentEnrollments(new StudentEnrollments()
            {
                StudentId = student_data.Id,
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
        public async Task<IActionResult> Delete(Guid classGuid, Guid studentGuid)
        {
            var class_data = _classesData.GetClassesById(classGuid);
            var student_data = _studentsData.GetStudentsById(studentGuid);

            var enrollment = _studentEnrollmentsData.GetStudentEnrollmentByClassAndStudent(classGuid, studentGuid);
            if (enrollment == null)
            {
                return Json(new { success = false, message = "Enrollment not found." });
            }

            _studentEnrollmentsData.DeleteStudentEnrollmentsById(enrollment.Id);

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
