namespace smtOffice.Domain.Entity
{
    public class Employee
    {
        public int ID { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Subdivision { get; set; } = default!;
        public string Position { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int PeoplePartnerID { get; set; }
        public int? ProjectID { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public byte[]? Photo { get; set; }
    }
}
