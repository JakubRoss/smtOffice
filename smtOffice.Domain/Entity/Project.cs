namespace smtOffice.Domain.Entity
{
    public class Project
    {
        public int ID { get; set; }
        public string ProjectType { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProjectManagerID { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = default!;
    }
}
