using BusinessLayer.Models;

namespace DataAccessLayer.DataAccess
{
	public interface ISubmissionsData
	{
		long InsertSubmissions(Submissions submissions);
		List<Submissions> GetAllSubmissions();
		Submissions GetSubmissionsById(long id);
		void DeleteSubmissionsById(long id);
		void UpdateSubmissionsById(Submissions submissions);
	}
}
