using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Domain.Entity;
using smtOffice.Application.Interfaces.Repository;

namespace smtoffice.Infrastructure.Repository
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public EmployeeRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            const string sql = @"INSERT INTO Employees (Username, PasswordHash, FullName, Subdivision, Position, Status, PeoplePartnerID, ProjectID, OutOfOfficeBalance, Photo)
                                 VALUES (@Username, @PasswordHash, @FullName, @Subdivision, @Position, @Status, @PeoplePartnerID, @ProjectID, @OutOfOfficeBalance, @Photo)";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Username", employee.Username);
            command.Parameters.AddWithValue("@PasswordHash", employee.PasswordHash);
            command.Parameters.AddWithValue("@FullName", employee.FullName);
            command.Parameters.AddWithValue("@Subdivision", employee.Subdivision);
            command.Parameters.AddWithValue("@Position", employee.Position);
            command.Parameters.AddWithValue("@Status", employee.Status);
            command.Parameters.AddWithValue("@PeoplePartnerID", employee.PeoplePartnerID);
            command.Parameters.AddWithValue("@ProjectID", employee.ProjectID ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@OutOfOfficeBalance", employee.OutOfOfficeBalance);
            command.Parameters.AddWithValue("@Photo", employee.Photo ?? (object)DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Employee>> ReadEmployeesAsync()
        {
            const string sql = "SELECT * FROM Employees";
            var employees = new List<Employee>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Subdivision = reader.GetString(reader.GetOrdinal("Subdivision")),
                    Position = reader.GetString(reader.GetOrdinal("Position")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    PeoplePartnerID = reader.GetInt32(reader.GetOrdinal("PeoplePartnerID")),
                    ProjectID = reader.IsDBNull(reader.GetOrdinal("ProjectID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProjectID")),
                    OutOfOfficeBalance = reader.GetInt32(reader.GetOrdinal("OutOfOfficeBalance")),
                    Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"]
                });
            }

            return employees;
        }

        public async Task<IEnumerable<Employee>> ReadEmployeesAsync(string position)
        {
            const string sql = "SELECT * FROM Employees WHERE Position = @Position";
            var employees = new List<Employee>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Position", position);
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Subdivision = reader.GetString(reader.GetOrdinal("Subdivision")),
                    Position = reader.GetString(reader.GetOrdinal("Position")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    PeoplePartnerID = reader.GetInt32(reader.GetOrdinal("PeoplePartnerID")),
                    ProjectID = reader.IsDBNull(reader.GetOrdinal("ProjectID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProjectID")),
                    OutOfOfficeBalance = reader.GetInt32(reader.GetOrdinal("OutOfOfficeBalance")),
                    Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"]
                });
            }

            return employees;
        }

        public async Task<Employee?> ReadEmployeeAsync(int employeeID)
        {
            const string sql = "SELECT * FROM Employees WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ID", employeeID);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Subdivision = reader.GetString(reader.GetOrdinal("Subdivision")),
                    Position = reader.GetString(reader.GetOrdinal("Position")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    PeoplePartnerID = reader.GetInt32(reader.GetOrdinal("PeoplePartnerID")),
                    ProjectID = reader.IsDBNull(reader.GetOrdinal("ProjectID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProjectID")),
                    OutOfOfficeBalance = reader.GetInt32(reader.GetOrdinal("OutOfOfficeBalance")),
                    Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"]
                };
            }

            return null;
        }

        public async Task<Employee?> ReadEmployeeAsync(string username)
        {
            const string sql = "SELECT * FROM Employees WHERE Username = @Username";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Subdivision = reader.GetString(reader.GetOrdinal("Subdivision")),
                    Position = reader.GetString(reader.GetOrdinal("Position")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    PeoplePartnerID = reader.GetInt32(reader.GetOrdinal("PeoplePartnerID")),
                    ProjectID = reader.IsDBNull(reader.GetOrdinal("ProjectID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProjectID")),
                    OutOfOfficeBalance = reader.GetInt32(reader.GetOrdinal("OutOfOfficeBalance")),
                    Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"]
                };
            }

            return null;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            const string sql = @"UPDATE Employees
                                 SET Username = @Username, PasswordHash = @PasswordHash, FullName = @FullName, Subdivision = @Subdivision,
                                     Position = @Position, Status = @Status, PeoplePartnerID = @PeoplePartnerID, ProjectID = @ProjectID,
                                     OutOfOfficeBalance = @OutOfOfficeBalance, Photo = @Photo
                                 WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ID", employee.ID);
            command.Parameters.AddWithValue("@Username", employee.Username);
            command.Parameters.AddWithValue("@PasswordHash", employee.PasswordHash);
            command.Parameters.AddWithValue("@FullName", employee.FullName);
            command.Parameters.AddWithValue("@Subdivision", employee.Subdivision);
            command.Parameters.AddWithValue("@Position", employee.Position);
            command.Parameters.AddWithValue("@Status", employee.Status);
            command.Parameters.AddWithValue("@PeoplePartnerID", employee.PeoplePartnerID);
            command.Parameters.AddWithValue("@ProjectID", employee.ProjectID ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@OutOfOfficeBalance", employee.OutOfOfficeBalance);
            command.Parameters.AddWithValue("@Photo", employee.Photo ?? (object)DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            const string sql = "DELETE FROM Employees WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
