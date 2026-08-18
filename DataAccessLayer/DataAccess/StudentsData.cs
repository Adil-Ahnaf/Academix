using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class StudentsData : IStudentsData
	{
		private readonly IDataAccess _dataAccess;

		public StudentsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertStudents(Students students)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertStudents, new{ students.AspNetUserId, students.StudentCode, students.FullName, students.Gender, students.StudentGuid, students.CreatedDate, students.CreatedBy, students.IsActive });
		}

		public List<Students> GetAllStudents()
		{
			return _dataAccess.GetList<Students>(SpConstants.GetAllStudents);
		}

		public Students GetStudentsById(long id)
		{
			return _dataAccess.GetSingle<Students>(SpConstants.GetStudentsById, new{ id });
		}

		public void DeleteStudentsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteStudentsById, new{ id });
		}

		public void UpdateStudentsById(Students students)
		{
			_dataAccess.Execute(SpConstants.UpdateStudentsById, new{ students.Id, students.AspNetUserId, students.StudentCode, students.FullName, students.Gender, students.StudentGuid, students.ModifiedDate, students.ModifiedBy, students.IsActive });
		}

	}
}
