using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class SubmissionsViewModelAdd
	{
		[Display(Name = "Assignment Id")]
		public long AssignmentId { get; set; }
		[Display(Name = "Student Id")]
		public long StudentId { get; set; }
		[Display(Name = "File Name")]
		public string FileName { get; set; }
		[Display(Name = "File Path")]
		public string FilePath { get; set; }
		[Display(Name = "Marks")]
		public decimal? Marks { get; set; }
		[Display(Name = "Feedback")]
		public string? Feedback { get; set; }
		[Display(Name = "Submission Guid")]
		public Guid SubmissionGuid { get; set; }
	}
}
