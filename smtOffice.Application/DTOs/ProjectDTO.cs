using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace smtOffice.Application.DTOs
{
    public class ProjectDTO
    {
        public int ID { get; set; }
        public string ProjectType { get; set; } = default!;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public int ProjectManagerID { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = default!;

        public List<SelectListItem> projectmanagers { get; set; } = new List<SelectListItem>();
    }
}
