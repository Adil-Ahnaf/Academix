using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface ITeachersData
	{
		long InsertTeachers(Teachers teachers);
		List<Teachers> GetAllTeachers();
		Teachers GetTeachersById(long id);
		void DeleteTeachersById(long id);
		void UpdateTeachersById(Teachers teachers);
	}
}
