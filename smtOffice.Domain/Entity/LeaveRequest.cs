namespace smtOffice.Domain.Entity
{
    public class LeaveRequest
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string AbsenceReason { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = default!;
    }
}
