using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class TeacherEnrollmentsViewModelAdd
	{
		[Display(Name = "Teacher Id")]
		public long TeacherId { get; set; }
		[Display(Name = "Class Id")]
		public long ClassId { get; set; }
	}
}
