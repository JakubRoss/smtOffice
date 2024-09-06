using smtOffice.Domain.Common;

namespace smtOffice.Domain.Entity.dropdown
{
    public class Subdivision : IName
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
