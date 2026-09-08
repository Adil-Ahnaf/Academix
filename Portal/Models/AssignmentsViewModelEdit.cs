using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
	public class AssignmentsViewModelEdit
	{
        public Guid AssignmentGuid { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; }
        public string Title { get; set; }
        public IFormFile? FilePath { get; set; }
        public string? ExistingFilePath { get; set; }
        public string? Description { get; set; }
        public int Marks { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsPublish { get; set; }
        public Classes ClassInfo { get; set; }
    }
}
