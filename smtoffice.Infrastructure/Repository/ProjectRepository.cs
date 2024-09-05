using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Domain.Entity;

namespace smtoffice.Infrastructure.Repository
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProjectRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Project?> ReadProjectAsync(int id)//
        {
            const string query = "SELECT * FROM Projects WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapProject(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Project>> ReadProjectsAsync()//
        {
            const string query = "SELECT * FROM Projects";
            var projects = new List<Project>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                projects.Add(MapProject(reader));
            }

            return projects;
        }

        public async Task CreateProjectAsync(Project project)//
        {
            const string query = @"
                INSERT INTO Projects (ProjectType, StartDate, EndDate, ProjectManagerID, Comment, Status) 
                VALUES (@ProjectType, @StartDate, @EndDate, @ProjectManagerID, @Comment, @Status)";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
            command.Parameters.AddWithValue("@StartDate", project.StartDate);
            command.Parameters.AddWithValue("@EndDate", (object)project.EndDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@ProjectManagerID", project.ProjectManagerID);
            command.Parameters.AddWithValue("@Comment", (object)project.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", project.Status);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProjectAsync(Project project)//
        {
            const string query = @"
                UPDATE Projects 
                SET ProjectType = @ProjectType, StartDate = @StartDate, EndDate = @EndDate, 
                    ProjectManagerID = @ProjectManagerID, Comment = @Comment, Status = @Status 
                WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", project.ID);
            command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
            command.Parameters.AddWithValue("@StartDate", project.StartDate);
            command.Parameters.AddWithValue("@EndDate", (object)project.EndDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@ProjectManagerID", project.ProjectManagerID);
            command.Parameters.AddWithValue("@Comment", (object)project.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", project.Status);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteProjectAsync(int id)//
        {
            const string query = "DELETE FROM Projects WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task AddEmployeesToProjectAsync(int projectId, List<int> employeeIds)//
        {
            if (employeeIds == null || !employeeIds.Any())
            {
                return; // No employees to update
            }

            var employeeIdsString = string.Join(",", employeeIds);

            const string query = "UPDATE Employees SET ProjectID = @ProjectID WHERE ID IN ({0})";
            var sql = string.Format(query, employeeIdsString);

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProjectID", projectId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        private static Project MapProject(SqlDataReader reader)
        {
            return new Project
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                ProjectType = reader.GetString(reader.GetOrdinal("ProjectType")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                ProjectManagerID = reader.GetInt32(reader.GetOrdinal("ProjectManagerID")),
                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                Status = reader.GetString(reader.GetOrdinal("Status"))
            };
        }
    }
}
