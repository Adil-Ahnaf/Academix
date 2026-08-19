using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using System.Diagnostics;

namespace Portal.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ITeachersData _teachersData;
        private readonly IStudentsData _studentsData;
        private readonly IClassesData _classesData;
        private readonly ISubjectsData _subjectsData;

        public HomeController(ITeachersData teachersData, IStudentsData studentsData, IClassesData classesData, ISubjectsData subjectsData)
        {
            _teachersData = teachersData;
            _studentsData = studentsData;
            _classesData = classesData;
            _subjectsData = subjectsData;
        }

        public IActionResult Index()
        {
            var userName = User.Identity?.Name;
            ViewBag.UserName = userName;

            if (User.IsInRole("Admin"))
            {
                AdminDashboardViewModel model = new AdminDashboardViewModel();
                var teacher = _teachersData.GetAllTeachers();
                var student = _studentsData.GetAllStudents();
                var classes = _classesData.GetAllClasses();
                var subject = _subjectsData.GetAllSubjects();

                model.TotalTeacher = teacher.Count;
                model.TotalStudent = student.Count;
                model.TotalClass = classes.Count;
                model.TotalSubject = subject.Count;

                return View(model);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
