using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class SubjectsViewModelEdit
	{
		[Display(Name = "Id")]
		public long Id { get; set; }
		[Display(Name = "Name")]
		public string Name { get; set; }
		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
	}
}
