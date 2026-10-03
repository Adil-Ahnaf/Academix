using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class SubmissionsData : ISubmissionsData
	{
		private readonly IDataAccess _dataAccess;

		public SubmissionsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertSubmissions(Submissions submissions)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertSubmissions, new{ submissions.AssignmentId, submissions.StudentId, submissions.FileName, submissions.FilePath, submissions.SubmissionGuid, submissions.CreatedDate, submissions.CreatedBy, submissions.IsActive });
		}

		public List<Submissions> GetAllSubmissions()
		{
			return _dataAccess.GetList<Submissions>(SpConstants.GetAllSubmissions);
		}

		public Submissions GetSubmissionsById(long id)
		{
			return _dataAccess.GetSingle<Submissions>(SpConstants.GetSubmissionsById, new{ id });
		}

		public void DeleteSubmissionsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteSubmissionsById, new{ id });
		}

		public void UpdateSubmissionsById(Submissions submissions)
		{
			_dataAccess.Execute(SpConstants.UpdateSubmissionsById, new{ submissions.Id, submissions.AssignmentId, submissions.StudentId, submissions.FileName, submissions.FilePath, submissions.Marks, submissions.Feedback, submissions.SubmissionGuid, submissions.ModifiedDate, submissions.ModifiedBy, submissions.IsActive });
		}

        public List<Submissions> GetSubmissionsByStudentAspNetUserId(string aspNetUserId)
        {
            return _dataAccess.GetList<Submissions>(SpConstants.GetSubmissionsByStudentAspNetUserId, new { aspNetUserId });
        }

        public Submissions GetSubmissionBySubmissionGuid(Guid submissionGuid)
        {
            return _dataAccess.GetSingle<Submissions>(SpConstants.GetSubmissionBySubmissionGuid, new { submissionGuid });
        }

        public void UpdateSubmissionsBySubmissionGuid(Submissions submissions)
        {
            _dataAccess.Execute(SpConstants.UpdateSubmissionsBySubmissionGuid, new { submissions.FileName, submissions.FilePath, submissions.SubmissionGuid, submissions.ModifiedDate, submissions.ModifiedBy });
        }

        public List<Submissions> GetAllSubmissionsByAssignmentGuid(Guid assignmentGuid)
        {
            return _dataAccess.GetList<Submissions>(SpConstants.GetAllSubmissionsByAssignmentGuid, new { assignmentGuid });
        }

        public void SaveSubmissionMarkBySubmissionGuid(Submissions submissions)
		{
            _dataAccess.Execute(SpConstants.SaveSubmissionMarkBySubmissionGuid, new { submissions.Marks, submissions.Feedback, submissions.SubmissionGuid, submissions.ModifiedDate, submissions.ModifiedBy });
        }

        public Submissions GetAStudentAssignmentMark(Guid assignmentGuid, string aspNetUserId)
        {
            return _dataAccess.GetSingle<Submissions>(SpConstants.GetAStudentAssignmentMark, new { assignmentGuid, aspNetUserId });
        }
    }
}
