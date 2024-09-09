using smtOffice.Application.DTOs;

namespace smtOffice.Application.Interfaces.Services
{
    public interface ILeaveRequestService
    {
        /// <summary>
        /// Creates a new leave request based on the provided data transfer object.
        /// </summary>
        /// <param name="leaveRequestDTO">The data transfer object containing leave request details to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="leaveRequestDTO"/> is null.</exception>
        Task CreateLeaveRequestAsync(LeaveRequestDTO leaveRequestDTO);

        /// <summary>
        /// Retrieves all leave requests for a specific employee.
        /// </summary>
        /// <param name="employeeID">The ID of the employee whose leave requests are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an enumerable collection of <see cref="LeaveRequestDTO"/>.</returns>
        Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequestsAsync(int employeeID);

        /// <summary>
        /// Retrieves a specific leave request by its ID for a specific employee.
        /// </summary>
        /// <param name="leaveRequestID">The ID of the leave request to be retrieved.</param>
        /// <param name="employeeID">The ID of the employee who owns the leave request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a <see cref="LeaveRequestDTO"/> if found; otherwise, null.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="leaveRequestID"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the leave request does not belong to the specified employee.</exception>
        Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int leaveRequestID);

        /// <summary>
        /// Deletes a leave request if it is in a "New" status and belongs to the specified employee.
        /// </summary>
        /// <param name="leaveRequestID">The ID of the leave request to be deleted.</param>
        /// <param name="employeeID">The ID of the employee who owns the leave request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="leaveRequestID"/> is less than or equal to zero.</exception>
        Task DeleteLeaveRequestAsync(int leaveRequestID, int employeeID);
    }
}
