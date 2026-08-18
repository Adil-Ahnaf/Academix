using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
	public class AssignmentsController : BaseController
	{
		private readonly IAssignmentsData _assignmentsData;
		private readonly IExportService _exportService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public AssignmentsController (IAssignmentsData assignmentsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
		{
			this._assignmentsData = assignmentsData;
			this._exportService = exportService;
			this._hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add()
		{
			var model = new AssignmentsViewModelAdd();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Insert(AssignmentsViewModelAdd model)
		{
            
            long assignmentsId = _assignmentsData.InsertAssignments(new Assignments()
            {
                TeacherEnrollmentId = model.TeacherEnrollmentId,
				Title = model.Title,
				Description = model.Description,
				Marks = model.Marks,
				Deadline = model.Deadline,
				IsPublish = model.IsPublish,
				AssignmentGuid = model.AssignmentGuid,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "Assignments");
		}
		public IActionResult Edit(int id)
		{
            AssignmentsViewModelEdit model = new AssignmentsViewModelEdit();
            var assignments = _assignmentsData.GetAssignmentsById(id);
            if (assignments != null)
            {
                model.Id = assignments.Id;
				model.TeacherEnrollmentId = assignments.TeacherEnrollmentId;
				model.Title = assignments.Title;
				model.Description = assignments.Description;
				model.Marks = assignments.Marks;
				model.Deadline = assignments.Deadline;
				model.IsPublish = assignments.IsPublish;
				model.AssignmentGuid = assignments.AssignmentGuid;
				model.IsActive = assignments.IsActive;
            }
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> Update(AssignmentsViewModelEdit model)
		{
            
            _assignmentsData.UpdateAssignmentsById(new Assignments()
            {
                Id = model.Id,
				TeacherEnrollmentId = model.TeacherEnrollmentId,
				Title = model.Title,
				Description = model.Description,
				Marks = model.Marks,
				Deadline = model.Deadline,
				IsPublish = model.IsPublish,
				AssignmentGuid = model.AssignmentGuid,
				IsActive = model.IsActive,
				ModifiedDate = DateTime.Now
            });
			return RedirectToAction("Index", "Assignments");
		}
        [HttpPost("Assignments/LoadTable")]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = _assignmentsData.GetAllAssignments().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.TeacherEnrollmentId != null && r.TeacherEnrollmentId.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Title != null && r.Title.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Description != null && r.Description.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Marks != null && r.Marks.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Deadline != null && r.Deadline.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.IsPublish != null && r.IsPublish.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.AssignmentGuid != null && r.AssignmentGuid.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Assignments>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
	}
}
