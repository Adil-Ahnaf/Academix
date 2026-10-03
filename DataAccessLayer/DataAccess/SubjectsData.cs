using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class SubjectsData : ISubjectsData
	{
		private readonly IDataAccess _dataAccess;

		public SubjectsData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertSubjects(Subjects subjects)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertSubjects, new{ subjects.Name, subjects.CreatedDate, subjects.CreatedBy, subjects.IsActive });
		}

		public List<Subjects> GetAllSubjects()
		{
			return _dataAccess.GetList<Subjects>(SpConstants.GetAllSubjects);
		}

		public Subjects GetSubjectsById(long id)
		{
			return _dataAccess.GetSingle<Subjects>(SpConstants.GetSubjectsById, new{ id });
		}

		public void DeleteSubjectsById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteSubjectsById, new{ id });
		}

		public void UpdateSubjectsById(Subjects subjects)
		{
			_dataAccess.Execute(SpConstants.UpdateSubjectsById, new{ subjects.Id, subjects.Name, subjects.ModifiedDate, subjects.ModifiedBy, subjects.IsActive });
		}

	}
}
