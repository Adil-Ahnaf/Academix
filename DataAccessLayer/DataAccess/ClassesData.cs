using BusinessLayer.Models;
using DataAccessLayer.SqlDb;

namespace DataAccessLayer.DataAccess
{
	public class ClassesData : IClassesData
	{
		private readonly IDataAccess _dataAccess;

		public ClassesData(IDataAccess dataAccess, DbConnectionInfo dbConnectionString)
		{
			_dataAccess = dataAccess;
			_dataAccess.ConnectionKey = dbConnectionString.Key;
		}

		public long InsertClasses(Classes classes)
		{
			return _dataAccess.ExecuteScalar<long>(SpConstants.InsertClasses, new{ classes.ClassName, classes.Section, classes.AcademicYear, classes.MaxCapacity, classes.ClassGuid, classes.CreatedDate, classes.CreatedBy, classes.IsActive });
		}

		public List<Classes> GetAllClasses()
		{
			return _dataAccess.GetList<Classes>(SpConstants.GetAllClasses);
		}

		public Classes GetClassesById(long id)
		{
			return _dataAccess.GetSingle<Classes>(SpConstants.GetClassesById, new{ id });
		}

		public void DeleteClassesById(long id)
		{
			_dataAccess.Execute(SpConstants.DeleteClassesById, new{ id });
		}

		public void UpdateClassesById(Classes classes)
		{
			_dataAccess.Execute(SpConstants.UpdateClassesById, new{ classes.Id, classes.ClassName, classes.Section, classes.AcademicYear, classes.MaxCapacity, classes.ClassGuid, classes.ModifiedDate, classes.ModifiedBy, classes.IsActive });
		}

	}
}
