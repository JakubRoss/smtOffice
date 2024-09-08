using smtOffice.Application.DTOs;
using smtOffice.Domain.Entity;

namespace smtOffice.Application.Interfaces.Services
{
    public interface IApprovalRequestService
    {
        /// <summary>
        /// Creates a new ApprovalRequest.
        /// </summary>
        /// <param name="approvalRequest">The approval request entity to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest);

        /// <summary>
        /// Retrieves an ApprovalRequest by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the approval request.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the 
        /// ApprovalRequest entity if found, or null if not.
        /// </returns>
        Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id);
        /// <summary>
        /// Retrieves all ApprovalRequests for a specific approver.
        /// </summary>
        /// <param name="approverID">The unique identifier of the approver.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of all 
        /// ApprovalRequest entities associated with the specified approver.
        /// </returns>
        Task<IEnumerable<ApprovalRequestDTO>> GetAllApprovalRequestsAsync(int approverID);
    }
}
