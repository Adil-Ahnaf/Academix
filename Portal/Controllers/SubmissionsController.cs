using BusinessLayer.Models;
using Portal.Models;
using DataAccessLayer.DataAccess;
using BusinessLayer.Services.ExportService;
using Microsoft.AspNetCore.Mvc;
using Portal.Extensions;
using Portal.Models.DatatableModels;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize]
    public class SubmissionsController : BaseController
	{
		private readonly ISubmissionsData _submissionsData;
		private readonly IExportService _exportService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public SubmissionsController (ISubmissionsData submissionsData, IExportService exportService, IWebHostEnvironment hostingEnvironment)
		{
			this._submissionsData = submissionsData;
			this._exportService = exportService;
			this._hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Add()
		{
			var model = new SubmissionsViewModelAdd();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Insert(SubmissionsViewModelAdd model)
		{
            
            long submissionsId = _submissionsData.InsertSubmissions(new Submissions()
            {
                AssignmentId = model.AssignmentId,
				StudentId = model.StudentId,
				FileName = model.FileName,
				FilePath = model.FilePath,
				Marks = model.Marks,
				Feedback = model.Feedback,
				SubmissionGuid = model.SubmissionGuid,
				CreatedBy = UserGuid,
				CreatedDate = DateTime.Now,
				IsActive = true
            });
			return RedirectToAction("Index", "Submissions");
		}
		public IActionResult Edit(int id)
		{
            SubmissionsViewModelEdit model = new SubmissionsViewModelEdit();
            var submissions = _submissionsData.GetSubmissionsById(id);
            if (submissions != null)
            {
                model.Id = submissions.Id;
				model.AssignmentId = submissions.AssignmentId;
				model.StudentId = submissions.StudentId;
				model.FileName = submissions.FileName;
				model.FilePath = submissions.FilePath;
				model.Marks = submissions.Marks;
				model.Feedback = submissions.Feedback;
				model.SubmissionGuid = submissions.SubmissionGuid;
				model.IsActive = submissions.IsActive;
            }
            return View(model);
		}
        [HttpPost]
        public async Task<IActionResult> Update(SubmissionsViewModelEdit model)
		{
            
            _submissionsData.UpdateSubmissionsById(new Submissions()
            {
                Id = model.Id,
				AssignmentId = model.AssignmentId,
				StudentId = model.StudentId,
				FileName = model.FileName,
				FilePath = model.FilePath,
				Marks = model.Marks,
				Feedback = model.Feedback,
				SubmissionGuid = model.SubmissionGuid,
				IsActive = model.IsActive,
				ModifiedBy = UserGuid,
				ModifiedDate = DateTime.Now
            });
			return RedirectToAction("Index", "Submissions");
		}
        [HttpPost("Submissions/LoadTable")]
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

            var result = _submissionsData.GetAllSubmissions().AsQueryable();
            var totalResultsCount = result.Count();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.AssignmentId != null && r.AssignmentId.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.StudentId != null && r.StudentId.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.FileName != null && r.FileName.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.FilePath != null && r.FilePath.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Marks != null && r.Marks.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.Feedback != null && r.Feedback.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.SubmissionGuid != null && r.SubmissionGuid.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedDate != null && r.ModifiedDate.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.ModifiedBy != null && r.ModifiedBy.ToString().ToUpper().Contains(searchBy.ToUpper())||
						r.IsActive != null && r.IsActive.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();


            return Json(new DtResult<Submissions>
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
