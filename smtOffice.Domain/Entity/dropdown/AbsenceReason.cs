using smtOffice.Domain.Common;

namespace smtOffice.Domain.Entity.dropdown
{
    public class AbsenceReason : IName
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
