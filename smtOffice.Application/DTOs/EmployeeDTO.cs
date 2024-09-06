using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace smtOffice.Application.DTOs
{
    public class EmployeeDTO
    {
        public int ID { get; set; }
        public string Username { get; set; } = default!;
        [ValidateNever]
        public string PasswordHash { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Subdivision { get; set; } = default!;
        public string Position { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int PeoplePartnerID { get; set; }
        public int? ProjectID { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public byte[]? Photo { get; set; }

        public List<SelectListItem> Subdivisions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Positions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AbsenceReasons { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> HRManagers { get; set; } = new List<SelectListItem>();
    }
}
