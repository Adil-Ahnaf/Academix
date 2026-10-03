using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class TeacherEnrollmentsData : ITeacherEnrollmentsData
	{
		private readonly IDataAccess _dataAccess;

		public TeacherEnrollmentsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertTeacherEnrollments(TeacherEnrollments teacherenrollments)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertTeacherEnrollments, new{ teacherenrollments.TeacherId, teacherenrollments.ClassId, teacherenrollments.CreatedDate, teacherenrollments.IsActive });
		}

		public List<TeacherEnrollments> GetAllTeacherEnrollments()
		{
			return _dataAccess.GetList<TeacherEnrollments>(SpConstants.GetAllTeacherEnrollments);
		}

		public TeacherEnrollments GetTeacherEnrollmentsById(long id)
		{
			return _dataAccess.GetSingle<TeacherEnrollments>(SpConstants.GetTeacherEnrollmentsById, new{ id });
		}

		public void DeleteTeacherEnrollmentsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteTeacherEnrollmentsById, new{ id });
		}

		public void UpdateTeacherEnrollmentsById(TeacherEnrollments teacherenrollments)
		{
			_dataAccess.Execute(SpConstants.UpdateTeacherEnrollmentsById, new{ teacherenrollments.Id, teacherenrollments.TeacherId, teacherenrollments.ClassId, teacherenrollments.ModifiedDate, teacherenrollments.IsActive });
		}

        public List<Classes> GetATeacherAllEnrollments(Guid teacherGuid)
        {
            return _dataAccess.GetList<Classes>(SpConstants.GetATeacherAllEnrollments, new { teacherGuid });
        }

        public TeacherEnrollments GetTeacherEnrollmentByClassAndTeacher(Guid classGuid, Guid teacherGuid)
        {
            return _dataAccess.GetSingle<TeacherEnrollments>(SpConstants.GetTeacherEnrollmentByClassAndTeacher, new { classGuid, teacherGuid });
        }
    }
}
