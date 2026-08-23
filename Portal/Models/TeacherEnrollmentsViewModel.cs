using BusinessLayer.Models;

namespace Portal.Models
{
    public class TeacherEnrollmentsViewModel
    {
        public Teachers? Teacher { get; set; }
        public List<TeacherEnrollments>? TeacherEnrollmentsList { get; set; }
        public List<Classes>? ClassesList { get; set; }
    }
}
