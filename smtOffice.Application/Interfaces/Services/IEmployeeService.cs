using smtOffice.Application.DTOs;

namespace smtOffice.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Asynchronously creates a new employee record.
        /// </summary>
        /// <param name="employee">An instance of <see cref="EmployeeDTO"/> containing employee data to be created.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="employee"/> parameter is <c>null</c>.</exception>
        Task CreateEmployeeAsync(EmployeeDTO employee);

        /// <summary>
        /// Asynchronously retrieves all employee records.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="EmployeeDTO"/> representing all employee records.
        /// </returns>
        Task<IEnumerable<EmployeeDTO>> ReadEmployeesAsync();

        /// <summary>
        /// Asynchronously retrieves a single employee record by its unique identifier.
        /// </summary>
        /// <param name="employeeID">The unique identifier of the employee.</param>
        /// <returns>
        /// An instance of <see cref="EmployeeDTO"/> representing the employee record with the specified ID, 
        /// or <c>null</c> if no employee with the specified ID is found.
        /// </returns>
        Task<EmployeeDTO> ReadEmployeeAsync(int employeeID);
        /// <summary>
        /// Asynchronously retrieves a single employee record by its unique identifier.
        /// </summary>
        /// <param name="position">The position of the employee.</param>
        /// <returns>
        /// An instance of <see cref="EmployeeDTO"/> representing the employee record with the specified Position, 
        /// or <c>null</c> if no employee with the specified Position is found.
        /// </returns>
        Task<List<EmployeeDTO>> ReadEmployeeAsync(string position);
        /// <summary>
        /// Asynchronously retrieves a single employee record by its unique username.
        /// </summary>
        /// <param name="userName">The username of the employee.</param>
        /// An instance of <see cref="EmployeeDTO"/> representing the employee record with the specified username, 
        /// or <c>null</c> if no employee with the specified username is found.
        /// </returns>
        Task<EmployeeDTO?> ReadEmployeeByUsernameAsync(string userName);
        /// <summary>
        /// Asynchronously updates an existing employee record.
        /// </summary>
        /// <param name="employeeDTO">An instance of <see cref="EmployeeDTO"/> containing the updated employee data.</param>
        /// <exception cref="ArgumentException">Thrown when no employee with the specified ID is found.</exception>
        Task UpdateEmployeeAsync(EmployeeDTO employeeDTO);

        /// <summary>
        /// Asynchronously deletes an employee record by its unique identifier.
        /// </summary>
        /// <param name="employeeID">The unique identifier of the employee to be deleted.</param>
        Task DeleteEmployeeAsync(int employeeID);
    }
}