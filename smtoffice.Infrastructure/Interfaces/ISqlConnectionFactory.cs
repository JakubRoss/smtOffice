using Microsoft.Data.Sqlite;

namespace smtoffice.Infrastructure.Interfaces
{
    public interface ISqlConnectionFactory
    {
        SqliteConnection CreateConnection();
    }
}
