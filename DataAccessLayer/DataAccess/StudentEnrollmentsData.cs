using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class StudentEnrollmentsData : IStudentEnrollmentsData
	{
		private readonly IDataAccess _dataAccess;

		public StudentEnrollmentsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertStudentEnrollments(StudentEnrollments studentenrollments)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertStudentEnrollments, new{ studentenrollments.StudentId, studentenrollments.ClassId, studentenrollments.CreatedDate, studentenrollments.IsActive });
		}

		public List<StudentEnrollments> GetAllStudentEnrollments()
		{
			return _dataAccess.GetList<StudentEnrollments>(SpConstants.GetAllStudentEnrollments);
		}

		public StudentEnrollments GetStudentEnrollmentsById(long id)
		{
			return _dataAccess.GetSingle<StudentEnrollments>(SpConstants.GetStudentEnrollmentsById, new{ id });
		}

		public void DeleteStudentEnrollmentsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteStudentEnrollmentsById, new{ id });
		}

		public void UpdateStudentEnrollmentsById(StudentEnrollments studentenrollments)
		{
			_dataAccess.Execute(SpConstants.UpdateStudentEnrollmentsById, new{ studentenrollments.Id, studentenrollments.StudentId, studentenrollments.ClassId, studentenrollments.ModifiedDate, studentenrollments.IsActive });
		}

        public List<Classes> GetAStudentAllEnrollments(Guid studentGuid)
        {
            return _dataAccess.GetList<Classes>(SpConstants.GetAStudentAllEnrollments, new { studentGuid });
        }
		
        public StudentEnrollments GetStudentEnrollmentByClassAndStudent(Guid classGuid, Guid studentGuid)
        {
            return _dataAccess.GetSingle<StudentEnrollments>(SpConstants.GetStudentEnrollmentByClassAndStudent, new { classGuid, studentGuid });
        }

        public bool CheckAStudentClassEnrollCapacity(Guid classGuid)
		{
            return _dataAccess.GetSingle<bool>(SpConstants.CheckAStudentClassEnrollCapacity, new { classGuid });
        }
    }
}
