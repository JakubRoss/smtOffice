namespace smtOffice.Application.DTOs
{
    public class ApprovalLeaveRequestDTO
    {
        public ApprovalRequestDTO ApprovalRequest { get; set; } = new ApprovalRequestDTO();
        public LeaveRequestDTO LeaveRequest { get; set; } = new LeaveRequestDTO();
        public string? FullName { get; set; }
    }

}
