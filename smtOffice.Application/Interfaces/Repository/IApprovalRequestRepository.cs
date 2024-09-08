using smtOffice.Domain.Entity;

namespace smtOffice.Application.Interfaces.Repository
{
    public interface IApprovalRequestRepository
    {
        /// <summary>
        /// Creates a new ApprovalRequest in the database.
        /// </summary>
        /// <param name="approvalRequest">The ApprovalRequest object to be created.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest);

        /// <summary>
        /// Retrieves an ApprovalRequest by its ID.
        /// </summary>
        /// <param name="id">The ID of the ApprovalRequest to retrieve.</param>
        /// <returns>
        /// A Task representing the asynchronous operation, with the ApprovalRequest object if found, or null if not.
        /// </returns>
        Task<ApprovalRequest?> GetApprovalRequestByIdAsync(int id);

        /// <summary>
        /// Retrieves all ApprovalRequests from the database.
        /// </summary>
        /// <returns>
        /// A Task representing the asynchronous operation, with a list of ApprovalRequest objects.
        /// </returns>
        Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync();
        /// <summary>
        /// Retrieves all ApprovalRequests for a specific approver.
        /// </summary>
        /// <param name="approverID">The unique identifier of the approver.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of all 
        /// ApprovalRequest entities associated with the specified approver.
        /// </returns>
        Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync(int approverID);

        /// <summary>
        /// Updates an existing ApprovalRequest in the database.
        /// </summary>
        /// <param name="approvalRequest">The ApprovalRequest object to update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task UpdateApprovalRequestAsync(ApprovalRequest approvalRequest);

        /// <summary>
        /// Deletes an ApprovalRequest from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the ApprovalRequest to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task DeleteApprovalRequestAsync(int id);
        /// <summary>
        /// Retrieves an ApprovalRequest by leave request ID.
        /// </summary>
        /// <param name="leaveRequestID">The ID of the LeaveRequest to retrieve.</param>
        /// <returns>
        /// A Task representing the asynchronous operation, with the ApprovalRequest object if found, or null if not.
        /// </returns>
        Task<ApprovalRequest?> GetApprovalRequestByLeaveRequestAsync(int leaveRequestID);
        /// <summary>
        /// Retrieves all approval requests for a specific approver asynchronously.
        /// </summary>
        /// <param name="approverID">The ID of the approver for whom approval requests are being fetched.</param>
        /// <returns>An enumerable collection of <see cref="ApprovalRequest"/> objects.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL query.</exception>
        /// <remarks>
        Task<ApprovalRequest?> GetApprovalRequestByLeaveRequestAsync(int leaveRequestID, int approverID);
    }
}