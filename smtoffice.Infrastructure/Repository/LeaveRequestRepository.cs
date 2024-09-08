using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Domain.Entity;

namespace smtoffice.Infrastructure.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public LeaveRequestRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Asynchronously creates a new leave request in the database.
        /// </summary>
        /// <param name="leaveRequest">The leave request object to be inserted into the database.</param>
        public async Task CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            const string sql = @"
                INSERT INTO LeaveRequests (EmployeeID, AbsenceReason, StartDate, EndDate, Comment, Status)
                VALUES (@EmployeeID, @AbsenceReason, @StartDate, @EndDate, @Comment, @Status)";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@EmployeeID", leaveRequest.EmployeeID);
            cmd.Parameters.AddWithValue("@AbsenceReason", leaveRequest.AbsenceReason);
            cmd.Parameters.AddWithValue("@StartDate", leaveRequest.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", leaveRequest.EndDate);
            cmd.Parameters.AddWithValue("@Comment", leaveRequest.Comment ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", leaveRequest.Status);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all leave requests from the database.
        /// </summary>
        /// <returns>A list of leave requests.</returns>
        public async Task<IEnumerable<LeaveRequest>> ReadLeaveRequestsAsync()
        {
            const string sql = "SELECT * FROM LeaveRequests";
            var leaveRequests = new List<LeaveRequest>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                leaveRequests.Add(MapLeaveRequest(reader));
            }

            return leaveRequests;
        }

        /// <summary>
        /// Asynchronously retrieves leave requests for a specific employee.
        /// </summary>
        /// <param name="employeeID">The ID of the employee whose leave requests are to be retrieved.</param>
        /// <returns>A list of leave requests for the specified employee.</returns>
        public async Task<IEnumerable<LeaveRequest>> ReadLeaveRequestsAsync(int employeeID)
        {
            const string sql = "SELECT * FROM LeaveRequests WHERE EmployeeID = @EmployeeID";
            var leaveRequests = new List<LeaveRequest>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                leaveRequests.Add(MapLeaveRequest(reader));
            }

            return leaveRequests;
        }

        /// <summary>
        /// Asynchronously retrieves a specific leave request by ID.
        /// </summary>
        /// <param name="leaveRequestID">The ID of the leave request to be retrieved.</param>
        /// <returns>The leave request with the specified ID, or null if not found.</returns>
        public async Task<LeaveRequest?> ReadLeaveRequestAsync(int leaveRequestID)
        {
            const string sql = "SELECT * FROM LeaveRequests WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@ID", leaveRequestID);

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapLeaveRequest(reader);
            }

            return null;
        }

        /// <summary>
        /// Asynchronously updates an existing leave request.
        /// </summary>
        /// <param name="leaveRequest">The leave request object containing updated information.</param>
        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            const string sql = @"
                UPDATE LeaveRequests
                SET EmployeeID = @EmployeeID, AbsenceReason = @AbsenceReason, StartDate = @StartDate, 
                    EndDate = @EndDate, Comment = @Comment, Status = @Status
                WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@ID", leaveRequest.ID);
            cmd.Parameters.AddWithValue("@EmployeeID", leaveRequest.EmployeeID);
            cmd.Parameters.AddWithValue("@AbsenceReason", leaveRequest.AbsenceReason);
            cmd.Parameters.AddWithValue("@StartDate", leaveRequest.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", leaveRequest.EndDate);
            cmd.Parameters.AddWithValue("@Comment", leaveRequest.Comment ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", leaveRequest.Status);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Asynchronously deletes a leave request by ID.
        /// </summary>
        /// <param name="leaveRequestID">The ID of the leave request to be deleted.</param>
        public async Task DeleteLeaveRequestAsync(int leaveRequestID)
        {
            const string sql = "DELETE FROM LeaveRequests WHERE ID = @ID";

            await using var connection = _sqlConnectionFactory.CreateConnection();
            await using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@ID", leaveRequestID);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static LeaveRequest MapLeaveRequest(SqlDataReader reader)
        {
            return new LeaveRequest
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                AbsenceReason = reader.IsDBNull(reader.GetOrdinal("AbsenceReason")) ? string.Empty : reader.GetString(reader.GetOrdinal("AbsenceReason")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString(reader.GetOrdinal("Status"))
            };
        }
    }
}
