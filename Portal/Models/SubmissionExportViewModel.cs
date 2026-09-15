using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class SubmissionExportViewModel
    {
        [Display(Name = "Student Id")]
        public string StudentCode { get; set; }
        [Display(Name = "Student Name")]
        public string FullName { get; set; }
        [Display(Name = "Marks")]
        public decimal? Marks { get; set; }
        [Display(Name = "Feedback")]
        public string Feedback { get; set; }
    }
}
