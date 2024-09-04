using Microsoft.Data.Sqlite;
using smtoffice.Infrastructure.Interfaces;

namespace smtoffice.Infrastructure.Common
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
