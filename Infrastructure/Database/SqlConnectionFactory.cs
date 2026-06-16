using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Database
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Database connection string is required.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
