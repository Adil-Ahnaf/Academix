using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface ISubjectsData
	{
		long InsertSubjects(Subjects subjects);
		List<Subjects> GetAllSubjects();
		Subjects GetSubjectsById(long id);
		void DeleteSubjectsById(long id);
		void UpdateSubjectsById(Subjects subjects);
	}
}
