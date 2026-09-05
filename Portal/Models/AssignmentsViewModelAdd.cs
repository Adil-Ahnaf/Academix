using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
	public class AssignmentsViewModelAdd
	{
		[Required]
		[Display(Name = "Teacher Enrollment Id")]
		public long TeacherEnrollmentId { get; set; }
		[Required]
		[Display(Name = "Title")]
		public string Title { get; set; }
		[Display(Name = "Description")]
		public string? Description { get; set; }
		[Required]
		[Display(Name = "Marks")]
		public int Marks { get; set; }
		[Required]
		[Display(Name = "Deadline")]
		public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);
        [Required]
		[Display(Name = "Is Publish")]
		public bool IsPublish { get; set; }
		[Display(Name = "Assignment Guid")]
		public Guid AssignmentGuid { get; set; }
        public Classes ClassInfo { get; set; }
        public string? ClassName { get; set; }
        public string? Section { get; set; }
        public long ClassId { get; set; }
        [Required]
        public IFormFile? FilePath { get; set; }
    }
}
