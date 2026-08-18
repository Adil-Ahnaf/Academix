using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class ClassesViewModelEdit
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Class Name")]
		public string ClassName { get; set; }
		[Display(Name = "Section")]
		public string Section { get; set; }
		[Display(Name = "Academic Year")]
		public string AcademicYear { get; set; }
		[Display(Name = "Max Capacity")]
		public int MaxCapacity { get; set; }
		[Display(Name = "Class Guid")]
		public Guid ClassGuid { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
