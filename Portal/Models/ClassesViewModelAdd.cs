using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class ClassesViewModelAdd
	{
		[Display(Name = "Class Name")]
		public string ClassName { get; set; }
        [Display(Name = "Subject")]
        public long SubjectId { get; set; }
        [Display(Name = "Section")]
		public string Section { get; set; }
		[Display(Name = "Academic Year")]
		public string AcademicYear { get; set; }
		[Display(Name = "Max Capacity")]
		public int MaxCapacity { get; set; }
		[Display(Name = "Class Guid")]
		public Guid ClassGuid { get; set; }
        public SelectList? SubjectOptions { get; set; }
    }
}
