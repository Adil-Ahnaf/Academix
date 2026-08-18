using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IStudentEnrollmentsData
	{
		long InsertStudentEnrollments(StudentEnrollments studentenrollments);
		List<StudentEnrollments> GetAllStudentEnrollments();
		StudentEnrollments GetStudentEnrollmentsById(long id);
		void DeleteStudentEnrollmentsById(long id);
		void UpdateStudentEnrollmentsById(StudentEnrollments studentenrollments);
	}
}
