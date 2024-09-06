using smtOffice.Application.DTOs;

namespace smtOffice.Application.Interfaces.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Verifies if the provided login credentials are valid.
        /// </summary>
        /// <param name="loginDTO">The <see cref="LoginDTO"/> containing the username and password provided by the user.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating whether the provided credentials are valid.
        /// Returns <c>true</c> if the credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsValidUser(LoginDTO loginDTO);
    }
}