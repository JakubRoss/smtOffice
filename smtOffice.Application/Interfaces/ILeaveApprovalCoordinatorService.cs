using smtOffice.Application.DTOs;

namespace smtOffice.Application.Interfaces
{
    public interface ILeaveApprovalCoordinatorService
    {
        Task ApproveLeaveRequestAsync(int leaveRequestID, int approverID, string approvalComment);
        Task RejectLeaveRequestAsync(int leaveRequestID, int approverID, string rejectionComment);
        Task SubmitRequestAsync(int leaveRequestID, int employeeID);
        Task<IEnumerable<ApprovalLeaveRequestDTO>> GetApprovalRequestsWithDetailsAsync(int approverID);
        Task<ApprovalLeaveRequestDTO> GetApprovalLeaveRequestDetailsAsync(int approvalID);
    }
}