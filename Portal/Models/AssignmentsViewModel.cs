using BusinessLayer.Models;

namespace Portal.Models
{
    public class AssignmentsViewModel
    {
        public List<Assignments> AllAssignment { get; set; }
        public int TotalAssignment { get; set; }
        public int PublishedAssignment { get; set; }
        public int DraftAssignment { get; set; }
    }
}
