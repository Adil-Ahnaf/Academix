using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class AssignmentsData : IAssignmentsData
	{
		private readonly IDataAccess _dataAccess;

		public AssignmentsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertAssignments(Assignments assignments)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertAssignments, new{ assignments.TeacherEnrollmentId, assignments.Title, assignments.Description, assignments.Marks, assignments.Deadline, assignments.IsPublish, assignments.AssignmentGuid, assignments.CreatedDate, assignments.IsActive });
		}

		public List<Assignments> GetAllAssignments()
		{
			return _dataAccess.GetList<Assignments>(SpConstants.GetAllAssignments);
		}

		public Assignments GetAssignmentsById(long id)
		{
			return _dataAccess.GetSingle<Assignments>(SpConstants.GetAssignmentsById, new{ id });
		}

		public void DeleteAssignmentsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteAssignmentsById, new{ id });
		}

		public void UpdateAssignmentsById(Assignments assignments)
		{
			_dataAccess.Execute(SpConstants.UpdateAssignmentsById, new{ assignments.Id, assignments.TeacherEnrollmentId, assignments.Title, assignments.Description, assignments.Marks, assignments.Deadline, assignments.IsPublish, assignments.AssignmentGuid, assignments.ModifiedDate, assignments.IsActive });
		}

	}
}
