using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class StudentEnrollmentsViewModelAdd
	{
		[Display(Name = "Student Id")]
		public long StudentId { get; set; }
		[Display(Name = "Class Id")]
		public long ClassId { get; set; }
	}
}
