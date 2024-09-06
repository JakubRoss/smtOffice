using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces;
using smtOffice.Application.Interfaces.Repository;
using smtOffice.Application.Interfaces.Services;

namespace smtOffice.Application.Services
{
    internal class AccountService(IPasswordHasher passwordHasher, IEmployeeRepository employeeRepository) : IAccountService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public async Task<bool> IsValidUser(LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Password) || string.IsNullOrWhiteSpace(loginDTO.Password))
                return false;
            var employee = await _employeeRepository.ReadEmployeeAsync(loginDTO.Username);
            if (employee == null || !_passwordHasher.VerifyPassword(loginDTO.Password, employee.PasswordHash))
                return false;
            return true;
        }
    }
}
