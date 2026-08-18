using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface IAssignmentsData
	{
		long InsertAssignments(Assignments assignments);
		List<Assignments> GetAllAssignments();
		Assignments GetAssignmentsById(long id);
		void DeleteAssignmentsById(long id);
		void UpdateAssignmentsById(Assignments assignments);
	}
}
