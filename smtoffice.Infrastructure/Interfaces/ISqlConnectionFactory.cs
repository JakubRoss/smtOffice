using Microsoft.Data.SqlClient;

namespace smtoffice.Infrastructure.Interfaces
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
