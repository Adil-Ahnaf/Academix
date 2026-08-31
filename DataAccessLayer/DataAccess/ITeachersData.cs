using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface ITeachersData
	{
		long InsertTeachers(Teachers teachers);
		List<Teachers> GetAllTeachers();
		Teachers GetTeachersById(Guid teacherGuid);
		void DeleteTeachersById(long id);
		void UpdateTeachersById(Teachers teachers);
		Teachers GetEnrolledTeacherByClassGuid(Guid classGuid);
		Teachers GetTeacherByAspNetUserId(string aspNetUserId);
	}
}
