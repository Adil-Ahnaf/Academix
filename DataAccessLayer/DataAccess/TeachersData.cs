using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class TeachersData : ITeachersData
	{
		private readonly IDataAccess _dataAccess;

		public TeachersData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertTeachers(Teachers teachers)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertTeachers, new{ teachers.AspNetUserId, teachers.FullName, teachers.Gender, teachers.Department, teachers.ProfileImage, teachers.TeacherGuid, teachers.CreatedDate, teachers.CreatedBy, teachers.IsActive });
		}

		public List<Teachers> GetAllTeachers()
		{
			return _dataAccess.GetList<Teachers>(SpConstants.GetAllTeachers);
		}

		public Teachers GetTeachersById(Guid teacherGuid)
		{
			return _dataAccess.GetSingle<Teachers>(SpConstants.GetTeachersById, new{ teacherGuid });
		}

		public void DeleteTeachersById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteTeachersById, new{ id });
		}

		public void UpdateTeachersById(Teachers teachers)
		{
			_dataAccess.Execute(SpConstants.UpdateTeachersById, new{ teachers.Id, teachers.AspNetUserId, teachers.FullName, teachers.Gender, teachers.Department, teachers.ProfileImage, teachers.TeacherGuid, teachers.ModifiedDate, teachers.ModifiedBy, teachers.IsActive });
		}
        public Teachers GetEnrolledTeacherByClassGuid(Guid classGuid)
        {
            return _dataAccess.GetSingle<Teachers>(SpConstants.GetEnrolledTeacherByClassGuid, new { classGuid });
        }
		
        public Teachers GetTeacherByAspNetUserId(string aspNetUserId)
        {
            return _dataAccess.GetSingle<Teachers>(SpConstants.GetTeacherByAspNetUserId, new { aspNetUserId });
        }

    }
}
