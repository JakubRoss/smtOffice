using smtOffice.Domain.Entity;

namespace smtoffice.Infrastructure.Repository
{
    public interface ILeaveRequestRepository
    {
        /// <summary>
        /// Creates a new leave request in the repository.
        /// </summary>
        /// <param name="leaveRequest">The leave request to be created.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateLeaveRequestAsync(LeaveRequest leaveRequest);

        /// <summary>
        /// Retrieves all leave requests from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of leave requests.</returns>
        Task<IEnumerable<LeaveRequest>> ReadLeaveRequestsAsync();
        /// <summary>
        /// Asynchronously retrieves a list of leave requests for a specific employee based on their ID.
        /// </summary>
        /// <param name="employeeID">The ID of the employee whose leave requests are being queried.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an 
        /// <see cref="IEnumerable{LeaveRequest}"/> representing the list of leave requests associated with the employee.
        /// </returns>
        Task<IEnumerable<LeaveRequest>> ReadLeaveRequestsAsync(int employeeID);

        /// <summary>
        /// Retrieves a leave request by its identifier.
        /// </summary>
        /// <param name="leaveRequestID">The identifier of the leave request to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the leave request if found, otherwise null.</returns>
        Task<LeaveRequest?> ReadLeaveRequestAsync(int leaveRequestID);

        /// <summary>
        /// Updates an existing leave request in the repository.
        /// </summary>
        /// <param name="leaveRequest">The leave request to be updated. The ID property must be set to identify which request to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);

        /// <summary>
        /// Deletes a leave request from the repository by its identifier.
        /// </summary>
        /// <param name="leaveRequestID">The identifier of the leave request to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteLeaveRequestAsync(int leaveRequestID);
    }

}