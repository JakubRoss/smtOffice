using smtoffice.Infrastructure.Repository;
using smtOffice.Domain.Common;

namespace smtOffice.Application.Services
{
    internal class DropDownService<T> : IDropDownService<T> where T : IName
    {
        private readonly IDropDownRepository _dropDownRepository;

        public DropDownService(IDropDownRepository dropDownRepository)
        {
            _dropDownRepository = dropDownRepository;
        }

        public async Task<List<string>> GetNamesFromModel(T model)
        {
            return await _dropDownRepository.GetNameFromTableAsync<T>();
        }
    }

}
