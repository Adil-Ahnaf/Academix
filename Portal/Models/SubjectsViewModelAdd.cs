using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class SubjectsViewModelAdd
	{
		[Display(Name = "Name")]
		public string Name { get; set; }
	}
}
