using BusinessLayer.Models;

namespace Portal.Models
{
    public class StudentEnrollmentsViewModel
    {
        public Students? Student { get; set; }
        public List<Classes>? StudentEnrollmentsList { get; set; }
        public List<Classes>? ClassesList { get; set; }
    }
}
