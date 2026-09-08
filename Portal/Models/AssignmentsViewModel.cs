using BusinessLayer.Models;

namespace Portal.Models
{
    public class AssignmentsViewModel
    {
        public Classes ClassInfo { get; set; }
        public List<Assignments> AllAssignment { get; set; }
        public int TotalAssignment { get; set; }
        public int PublishedAssignment { get; set; }
        public int DraftAssignment { get; set; }
        public int TotalSubmissions { get; set; }
        public int TotalStudents { get; set; }
    }
}
