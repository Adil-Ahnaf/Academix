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

        public HomeController(ITeachersData teachersData)
        {
           _teachersData = teachersData; 
        }

        public IActionResult Index()
        {
            var userName = User.Identity?.Name;
            ViewBag.UserName = userName;

            if (User.IsInRole("Admin"))
            {
                AdminDashboardViewModel model = new AdminDashboardViewModel();
                var teacher = _teachersData.GetAllTeachers();

                model.TotalTeacher = teacher.Count;
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
