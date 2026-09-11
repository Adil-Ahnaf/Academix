using BusinessLayer.Models;

namespace Portal.Models
{
    public class StudentDashboardViewModel
    {
        public Students Student { get; set; }
        public List<Classes>? AllClasses { get; set; }
    }
}
