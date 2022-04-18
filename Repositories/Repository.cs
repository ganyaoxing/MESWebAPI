using System.Data;
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MESWebAPI.Models;

namespace MESWebAPI.Repositories
{
#nullable disable
    public class Repository
    {
        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public Repository(string connectionString)
        {
            SetConnectionString(connectionString);
        }

        public Repository()
        {
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
    }
}