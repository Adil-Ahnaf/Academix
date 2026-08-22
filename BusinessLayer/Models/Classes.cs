using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Models
{
	public class Classes
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Class Name")]
		public string ClassName { get; set; }
        [Display(Name = "Subject Id")]
        public long SubjectId { get; set; }
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }
        [Display(Name = "Section")]
		public string Section { get; set; }
		[Display(Name = "Academic Year")]
		public string AcademicYear { get; set; }
		[Display(Name = "Max Capacity")]
		public int MaxCapacity { get; set; }
		[Display(Name = "Class Guid")]
		public Guid ClassGuid { get; set; }
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
