using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Domain.Common;

namespace smtoffice.Infrastructure.Repository
{
    internal class DropDownRepository(ISqlConnectionFactory sqlConnectionFactory) : IDropDownRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<List<string>> GetNameFromTableAsync<T>() where T : IName
        {
            var names = new List<string>();

            // Pobierz nazwę tabeli na podstawie typu T (zakładając, że nazwa tabeli jest taka sama jak nazwa klasy)
            string tableName = typeof(T).Name;

            // Tworzenie zapytania SQL z dynamiczną nazwą tabeli
            string query = $"SELECT Name FROM [{tableName}]";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                names.Add(reader["Name"].ToString());
            }

            return names;
        }

    }
}
