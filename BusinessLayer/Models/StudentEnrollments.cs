using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class StudentEnrollments
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Student Id")]
		public long StudentId { get; set; }
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
