using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class Teachers
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Asp Net User Id")]
		public string AspNetUserId { get; set; }
		[Display(Name = "Full Name")]
		public string FullName { get; set; }
		[Display(Name = "Gender")]
		public string Gender { get; set; }
		[Display(Name = "Department")]
		public string Department { get; set; }
		[Display(Name = "Profile Image")]
		public string? ProfileImage { get; set; }
		[Display(Name = "Teacher Guid")]
		public Guid TeacherGuid { get; set; }
		[Display(Name = "Created Date")]
		public DateTime? CreatedDate { get; set; }
		[Display(Name = "Created By")]
		public string? CreatedBy { get; set; }
		[Display(Name = "Modified Date")]
		public DateTime? ModifiedDate { get; set; }
		[Display(Name = "Modified By")]
		public string? ModifiedBy { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
