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
            bool enrollAccess = _studentEnrollmentsData.CheckAStudentClassEnrollCapacity(classGuid);

            if (!enrollAccess)
            {
                return Json(new 
                { 
                    success = false, 
                    message = "Enrollment failed. The class has reached its maximum capacity." 
                });
            }
            else
            {
                long classId = _classesData.GetClassesById(classGuid).Id;
                long studentId = _studentsData.GetStudentsById(studentGuid).Id;

                long studentEnrollmentsId = _studentEnrollmentsData.InsertStudentEnrollments(new StudentEnrollments()
                {
                    StudentId = studentId,
                    ClassId = classId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                });

                var enrolledClass = _classesData.GetStudentEnrollClassDetailsByEnrollmentId(studentEnrollmentsId);

                return Json(new
                {
                    success = true,
                    classGuid = enrolledClass.ClassGuid,
                    academicYear = enrolledClass.AcademicYear,
                    className = enrolledClass.ClassName,
                    subject = enrolledClass.SubjectName,
                    section = enrolledClass.Section,
                    maxCapacity = enrolledClass.MaxCapacity,
                    totalEnrolled = enrolledClass.TotalEnrolled
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid classGuid, Guid studentGuid)
        {
            var enrollment = _studentEnrollmentsData.GetStudentEnrollmentByClassAndStudent(classGuid, studentGuid);
            if (enrollment == null)
            {
                return Json(new { success = false, message = "Enrollment not found." });
            }
            else
            {
                var enrolledClass = _classesData.GetStudentEnrollClassDetailsByEnrollmentId(enrollment.Id);

                _studentEnrollmentsData.DeleteStudentEnrollmentsById(enrollment.Id);

                return Json(new
                {
                    success = true,
                    classGuid = enrolledClass.ClassGuid,
                    academicYear = enrolledClass.AcademicYear,
                    className = enrolledClass.ClassName,
                    subject = enrolledClass.SubjectName,
                    section = enrolledClass.Section,
                    maxCapacity = enrolledClass.MaxCapacity,
                    totalEnrolled = enrolledClass.TotalEnrolled > 1 ? enrolledClass.TotalEnrolled - 1 : 0,
                });
            }
        }
    }
}
