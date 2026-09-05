using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class Assignments
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Teacher Enrollment Id")]
		public long TeacherEnrollmentId { get; set; }
		[Display(Name = "Title")]
		public string Title { get; set; }
        [Display(Name = "File Path")]
        public string? FilePath { get; set; }
        [Display(Name = "Description")]
		public string? Description { get; set; }
		[Display(Name = "Marks")]
		public int Marks { get; set; }
		[Display(Name = "Deadline")]
		public DateTime Deadline { get; set; }
		[Display(Name = "Is Publish")]
		public bool IsPublish { get; set; }
		[Display(Name = "Assignment Guid")]
		public Guid AssignmentGuid { get; set; }
		[Display(Name = "Created Date")]
		public DateTime? CreatedDate { get; set; }
		[Display(Name = "Modified Date")]
		public DateTime? ModifiedDate { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
