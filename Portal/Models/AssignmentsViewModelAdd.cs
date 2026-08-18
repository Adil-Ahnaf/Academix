using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class AssignmentsViewModelAdd
	{
		[Display(Name = "Teacher Enrollment Id")]
		public long TeacherEnrollmentId { get; set; }
		[Display(Name = "Title")]
		public string Title { get; set; }
		[Display(Name = "Description")]
		public string Description { get; set; }
		[Display(Name = "Marks")]
		public int Marks { get; set; }
		[Display(Name = "Deadline")]
		public DateTime Deadline { get; set; }
		[Display(Name = "Is Publish")]
		public int IsPublish { get; set; }
		[Display(Name = "Assignment Guid")]
		public Guid AssignmentGuid { get; set; }
	}
}
