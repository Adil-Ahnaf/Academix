using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Models
{
	public class SubmissionsViewModel
	{
        public Guid SubmissionGuid { get; set; }

        public decimal? Marks { get; set; }

        public string? Feedback { get; set; }
    }
}
