using smtOffice.Domain.Common;

namespace smtoffice.Infrastructure.Repository
{
    public interface IDropDownRepository
    {
        Task<List<string>> GetNameFromTableAsync<T>() where T : IName;
    }
}