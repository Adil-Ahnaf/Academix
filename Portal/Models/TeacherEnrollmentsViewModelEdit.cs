using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class TeacherEnrollmentsViewModelEdit
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Teacher Id")]
		public long TeacherId { get; set; }
		[Display(Name = "Class Id")]
		public long ClassId { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
