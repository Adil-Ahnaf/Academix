using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.SqlDb
{
    public interface IDataAccess
    {
        string ConnectionKey { get; set; }
        string ConnectionString { get; set; }

        int Execute(string spName);
        int Execute(string spName, object param);
        T ExecuteScalar<T>(string spName);
        T ExecuteScalar<T>(string spName, object param);
        List<T> GetList<T>(string spName);
        List<T> GetList<T>(string spName, int id);
        List<T> GetList<T>(string spName, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultipleList<T1, T2>(string spName, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultipleList<T1, T2, T3>(string spName, object param);
        T GetSingle<T>(string spName, int id);
        T GetSingle<T>(string spName, object param);
    }
}
