using BusinessLayer.Models;
using BusinessLayer.Services;
using BusinessLayer.Services.ExportService;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.DatatableModels;

namespace Portal.Controllers
{
    [Authorize]
    public class ClassesController : BaseController
    {
        private readonly IClassesData _classesData;
        private readonly ISubjectsData _subjectsData;
        private readonly ITeachersData _teachersData;
        private readonly IStudentsData _studentsData;

        public ClassesController(IClassesData classesData, ISubjectsData subjectsData, ITeachersData teachersData, 
            IStudentsData studentsData)
        {
            _classesData = classesData;
            _subjectsData = subjectsData;
            _teachersData = teachersData;
            _studentsData = studentsData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new ClassesViewModelAdd();
            model.SubjectOptions = GetSubjectList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ClassesViewModelAdd model)
        {

            long classesId = _classesData.InsertClasses(new Classes()
            {
                ClassName = model.ClassName,
                SubjectId = model.SubjectId,
                Section = model.Section,
                AcademicYear = model.AcademicYear,
                MaxCapacity = model.MaxCapacity,
                ClassGuid = Guid.NewGuid(),
                CreatedBy = UserGuid,
                CreatedDate = DateTime.Now,
                IsActive = true
            });
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("Classes/Edit/{classGuid:guid}")]
        public IActionResult Edit(Guid classGuid)
        {
            ClassesViewModelEdit model = new ClassesViewModelEdit();
            var classes = _classesData.GetClassesById(classGuid);
            if (classes != null)
            {
                model.Id = classes.Id;
                model.ClassName = classes.ClassName;
                model.SubjectId = classes.SubjectId;
                model.Section = classes.Section;
                model.AcademicYear = classes.AcademicYear;
                model.MaxCapacity = classes.MaxCapacity;
                model.ClassGuid = classes.ClassGuid;
                model.IsActive = classes.IsActive;
                model.SubjectOptions = GetSubjectList();
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ClassesViewModelEdit model)
        {
            _classesData.UpdateClassesById(new Classes()
            {
                Id = model.Id,
                ClassName = model.ClassName,
                SubjectId = model.SubjectId,
                Section = model.Section,
                AcademicYear = model.AcademicYear,
                MaxCapacity = model.MaxCapacity,
                ClassGuid = model.ClassGuid,
                IsActive = model.IsActive,
                ModifiedBy = UserGuid,
                ModifiedDate = DateTime.Now
            });
            return RedirectToAction("Index", "Home");
        }
        [Route("Classes/EnrollmentDetails/{classGuid:guid}")]
        public async Task<IActionResult> EnrollmentDetails(Guid classGuid)
        {
            var model = new EnrollmentDetailsViewModel
            {
                EnrolledTeacher = _teachersData.GetEnrolledTeacherByClassGuid(classGuid),
                EnrolledClass = _classesData.GetClassesById(classGuid)
            };
            return View(model);
        }
        private SelectList GetSubjectList()
        {
            var subjectList = _subjectsData.GetAllSubjects();
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "Select", Value = "" });
            foreach (var item in subjectList)
            {
                selectListItems.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var selectList = new SelectList(selectListItems, "Value", "Text", 1);
            return selectList;
        }
        public IActionResult LoadTable([FromBody] DtParameters? dtParameters)
        {
            if (dtParameters == null)
            {
                return BadRequest(new
                {
                    error = "DataTables parameters are null."
                });
            }

            Guid? classGuid = dtParameters?.ClassGuid;
            string? searchBy = dtParameters.Search?.Value;
            string orderCriteria = "Id";
            bool orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0 && dtParameters.Columns != null)
            {
                int columnIndex = dtParameters.Order[0].Column;

                if (columnIndex >= 0 && columnIndex < dtParameters.Columns.Length)
                {
                    string? requestedColumn = dtParameters.Columns[columnIndex].Data;

                    if (!string.IsNullOrWhiteSpace(requestedColumn))
                    {
                        orderCriteria = requestedColumn;
                    }
                }
                orderAscendingDirection = !string.Equals(dtParameters.Order[0].Dir, "desc", StringComparison.OrdinalIgnoreCase);
            }

            var result = _studentsData.GetEnrolledStudentsByClassGuid(classGuid.Value).AsQueryable();

            int totalResultsCount = result.Count();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                string search = searchBy.Trim();

                result = result.Where(r =>
                    (r.StudentCode != null &&
                     r.StudentCode.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.FullName != null &&
                     r.FullName.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))

                    ||

                    (r.Gender != null &&
                     r.Gender.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))
                );
            }

            result = orderAscendingDirection
                ? result.OrderByDynamic(
                    orderCriteria,
                    DtOrderDir.Asc)

                : result.OrderByDynamic(
                    orderCriteria,
                    DtOrderDir.Desc);

            int filteredResultsCount = result.Count();

            int start = Math.Max(
                dtParameters.Start,
                0);

            int length = dtParameters.Length;


            if (length <= 0)
            {
                length = 10;
            }


            var data = result
                .Skip(start)
                .Take(length)
                .ToList();

            return Json(new DtResult<Students>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = data
            });
        }
    }
}
