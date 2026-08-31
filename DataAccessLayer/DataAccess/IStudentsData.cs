using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IStudentsData
	{
		long InsertStudents(Students students);
		List<Students> GetAllStudents();
		Students GetStudentsById(Guid studentGuid);
		void DeleteStudentsById(long id);
		void UpdateStudentsById(Students students);
		List<Students> GetEnrolledStudentsByClassGuid(Guid classGuid);
	}
}
