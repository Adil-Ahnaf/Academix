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
		List<Classes> GetATeacherAllEnrollments(Guid teacherGuid);
		TeacherEnrollments GetTeacherEnrollmentByClassAndTeacher(Guid classGuid, Guid teacherGuid);
	}
}
