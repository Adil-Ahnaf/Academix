using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IClassesData
	{
		long InsertClasses(Classes classes);
		List<Classes> GetAllClasses();
		Classes GetClassesById(Guid classGuid);
		void DeleteClassesById(long id);
		void UpdateClassesById(Classes classes);
		List<Classes> GetAllActiveClasses();
		List<Classes> GetAllActiveClassesForATeacher(Guid teacherGuid);
    }
}
