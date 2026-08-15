using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer.SqlDb
{
    internal class SqlDapperDataAccess : IDataAccess
    {
        public string ConnectionKey { get; set; }
        public string ConnectionString { get; set; }
        private IConfiguration _cofig { get; set; }

        public SqlDapperDataAccess(IConfiguration config, DbConnectionInfo dbConnection)
        {
            _cofig = config;
            ConnectionString = _cofig.GetConnectionString(dbConnection.Key);
        }

        public int Execute(string spName)
        {
            return Execute(spName, null);
        }

        public int Execute(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                return con.Execute(spName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public T ExecuteScalar<T>(string spName)
        {
            return ExecuteScalar<T>(spName, null);
        }

        public T ExecuteScalar<T>(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                return con.ExecuteScalar<T>(spName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public T GetSingle<T>(string spName, int id)
        {
            return GetSingle<T>(spName, new { id });
        }
        public T GetSingle<T>(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(spName, param, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.FirstOrDefault();
            }

            return default(T);
        }

        public List<T> GetList<T>(string spName, int id)
        {
            return GetList<T>(spName, new { id });
        }

        public List<T> GetList<T>(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {

                var result = con.Query<T>(spName, param, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.ToList();
            }

            return new List<T>();
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultipleList<T1, T2>(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.QueryMultiple(spName, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultipleList<T1, T2, T3>(string spName, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.QueryMultiple(spName, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                var item3 = result.Read<T3>().ToList();

                if (item1 != null && item2 != null && item3 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(new List<T1>(), new List<T2>(), new List<T3>());
        }

        public List<T> GetList<T>(string spName)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(spName, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.ToList();
            }
            return new List<T>();
        }
    }
}
