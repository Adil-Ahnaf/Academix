using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class Subjects
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Name")]
		public string Name { get; set; }
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
