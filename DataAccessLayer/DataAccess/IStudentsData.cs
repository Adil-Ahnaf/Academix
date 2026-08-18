using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IStudentsData
	{
		long InsertStudents(Students students);
		List<Students> GetAllStudents();
		Students GetStudentsById(long id);
		void DeleteStudentsById(long id);
		void UpdateStudentsById(Students students);
	}
}
