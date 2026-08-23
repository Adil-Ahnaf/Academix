using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface ITeacherEnrollmentsData
	{
		long InsertTeacherEnrollments(TeacherEnrollments teacherenrollments);
		List<TeacherEnrollments> GetAllTeacherEnrollments();
		TeacherEnrollments GetTeacherEnrollmentsById(long id);
		void DeleteTeacherEnrollmentsById(long id);
		void UpdateTeacherEnrollmentsById(TeacherEnrollments teacherenrollments);
		List<TeacherEnrollments> GetATeacherAllEnrollments(Guid teacherGuid);
	}
}
