using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Application.Interfaces.Repository;
using smtOffice.Domain.Entity;
using System.Data;

namespace smtOffice.Infrastructure.Repository
{
    public class ApprovalRequestRepository : IApprovalRequestRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ApprovalRequestRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Creates a new ApprovalRequest in the database.
        /// </summary>
        /// <param name="approvalRequest">ApprovalRequest object to be created.</param>
        public async Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest)
        {
            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = @"INSERT INTO ApprovalRequests (ApproverID, LeaveRequestID, Status, Comment)
                         VALUES (@ApproverID, @LeaveRequestID, @Status, @Comment)";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@ApproverID", SqlDbType.Int).Value = approvalRequest.ApproverID;
            cmd.Parameters.Add("@LeaveRequestID", SqlDbType.Int).Value = approvalRequest.LeaveRequestID;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = approvalRequest.Status ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = approvalRequest.Comment ?? (object)DBNull.Value;

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<ApprovalRequest?> GetApprovalRequestByIdAsync(int id)
        {
            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = @"SELECT * FROM ApprovalRequests WHERE ID = @ID";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ApprovalRequest
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    ApproverID = reader.GetInt32(reader.GetOrdinal("ApproverID")),
                    LeaveRequestID = reader.GetInt32(reader.GetOrdinal("LeaveRequestID")),
                    Status = reader["Status"].ToString() ?? string.Empty,
                    Comment = reader["Comment"] as string
                };
            }

            return null;
        }


        public async Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync()
        {
            var approvalRequests = new List<ApprovalRequest>();

            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = "SELECT * FROM ApprovalRequests";

            using var cmd = new SqlCommand(sql, connection);
            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                approvalRequests.Add(new ApprovalRequest
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    ApproverID = reader.GetInt32(reader.GetOrdinal("ApproverID")),
                    LeaveRequestID = reader.GetInt32(reader.GetOrdinal("LeaveRequestID")),
                    Status = reader["Status"].ToString() ?? string.Empty,
                    Comment = reader["Comment"] as string ?? string.Empty
                });
            }

            return approvalRequests;
        }
        public async Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync(int approverID)
        {
            var approvalRequests = new List<ApprovalRequest>();
            using var connection = _sqlConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM ApprovalRequests WHERE ApproverID = @approverID";

            using SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@approverID", approverID);
            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                approvalRequests.Add(new ApprovalRequest
                {
                    ID = (int)reader["ID"],
                    ApproverID = (int)reader["ApproverID"],
                    LeaveRequestID = (int)reader["LeaveRequestID"],
                    Status = reader["Status"].ToString() ?? string.Empty,
                    Comment = reader["Comment"]?.ToString() ?? string.Empty
                });
            }
            return approvalRequests;
        }

        public async Task UpdateApprovalRequestAsync(ApprovalRequest approvalRequest)
        {
            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = @"UPDATE ApprovalRequests 
                         SET ApproverID = @ApproverID, 
                             LeaveRequestID = @LeaveRequestID, 
                             Status = @Status, 
                             Comment = @Comment
                         WHERE ID = @ID";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = approvalRequest.ID;
            cmd.Parameters.Add("@ApproverID", SqlDbType.Int).Value = approvalRequest.ApproverID;
            cmd.Parameters.Add("@LeaveRequestID", SqlDbType.Int).Value = approvalRequest.LeaveRequestID;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = approvalRequest.Status ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = approvalRequest.Comment ?? (object)DBNull.Value;

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteApprovalRequestAsync(int id)
        {
            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = @"DELETE FROM ApprovalRequests WHERE ID = @ID";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<ApprovalRequest?> GetApprovalRequestByLeaveRequestAsync(int leaveRequestID)
        {
            await using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = @"SELECT * FROM ApprovalRequests WHERE LeaveRequestID = @leaveRequestID";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@leaveRequestID", SqlDbType.Int).Value = leaveRequestID;

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ApprovalRequest
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    ApproverID = reader.GetInt32(reader.GetOrdinal("ApproverID")),
                    LeaveRequestID = reader.GetInt32(reader.GetOrdinal("LeaveRequestID")),
                    Status = reader["Status"].ToString() ?? string.Empty,
                    Comment = reader["Comment"] as string
                };
            }

            return null;
        }
        public async Task<ApprovalRequest?> GetApprovalRequestByLeaveRequestAsync(int leaveRequestID, int approverID)
        {
            var connection = _sqlConnectionFactory.CreateConnection();

            await using (connection) 
            {
                string sql = @"SELECT * FROM ApprovalRequests WHERE LeaveRequestID = @leaveRequestID AND ApproverID = @approverID";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@leaveRequestID", SqlDbType.Int).Value = leaveRequestID;
                    cmd.Parameters.Add("@approverID", SqlDbType.Int).Value = approverID;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ApprovalRequest
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                ApproverID = reader.GetInt32(reader.GetOrdinal("ApproverID")),
                                LeaveRequestID = reader.GetInt32(reader.GetOrdinal("LeaveRequestID")),
                                Status = reader["Status"].ToString() ?? string.Empty,
                                Comment = reader["Comment"] != DBNull.Value ? reader["Comment"].ToString() : null
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
