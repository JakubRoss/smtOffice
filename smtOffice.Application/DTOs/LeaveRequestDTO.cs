using Microsoft.AspNetCore.Mvc.Rendering;

namespace smtOffice.Application.DTOs
{
    public class LeaveRequestDTO
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string AbsenceReason { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = "New";

        public List<SelectListItem> AbsenceReasons { get; set; } = new List<SelectListItem>();
    }
}
