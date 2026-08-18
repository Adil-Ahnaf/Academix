using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IClassesData
	{
		long InsertClasses(Classes classes);
		List<Classes> GetAllClasses();
		Classes GetClassesById(long id);
		void DeleteClassesById(long id);
		void UpdateClassesById(Classes classes);
	}
}
