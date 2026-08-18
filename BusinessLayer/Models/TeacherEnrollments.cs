using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class TeacherEnrollments
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Teacher Id")]
		public long TeacherId { get; set; }
		[Display(Name = "Class Id")]
		public long ClassId { get; set; }
		[Display(Name = "Created Date")]
		public DateTime? CreatedDate { get; set; }
		[Display(Name = "Modified Date")]
		public DateTime? ModifiedDate { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
