namespace smtOffice.Domain.Entity
{
    public class ApprovalRequest
    {
        public int ID { get; set; }
        public int ApproverID { get; set; }
        public int LeaveRequestID { get; set; }
        public string Status { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
