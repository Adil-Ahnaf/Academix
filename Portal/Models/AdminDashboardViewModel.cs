using BusinessLayer.Models;

namespace Portal.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalTeacher { get; set; }
        public int TotalStudent { get; set; }
        public int TotalClass { get; set; }
        public int TotalSubject { get; set; }
        public IEnumerable<Classes> Classes { get; set; } = new List<Classes>();
    }
}
