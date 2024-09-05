using smtOffice.Domain.Common;

namespace smtOffice.Application.Services
{
    public interface IDropDownService<T> where T : IName
    {
        Task<List<string>> GetNamesFromModel(T model);
    }
}