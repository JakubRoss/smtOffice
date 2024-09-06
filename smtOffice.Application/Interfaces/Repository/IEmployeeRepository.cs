using smtOffice.Domain.Entity;

namespace smtOffice.Application.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Asynchronously creates a new employee record in the database.
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> object containing the details of the employee to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Asynchronously reads all employee records from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{Employee}"/> containing all employees retrieved from the database.
        /// </returns>
        Task<IEnumerable<Employee>> ReadEmployeesAsync();
        /// <summary>
        /// Asynchronously retrieves a collection of employees from the database based on the specified position.
        /// </summary>
        /// <param name="position">The position of the employees to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of Employee objects.</returns>
        /// <remarks>
        /// This method creates a SQL connection using a factory, executes a query to fetch employees with the specified position, 
        /// and maps the results to a list of Employee objects. It ensures the database connection is properly managed with a using statement.
        /// </remarks>
        Task<IEnumerable<Employee>> ReadEmployeesAsync(string position);
        /// <summary>
        /// Asynchronously reads a single employee record from the database based on the provided employee ID.
        /// </summary>
        /// <param name="employeeID">The ID of the employee to search for.</param>
        /// <returns>
        /// The <see cref="Employee"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Employee?> ReadEmployeeAsync(int employeeID);

        /// <summary>
        /// Asynchronously reads an employee record from the database based on the provided username.
        /// </summary>
        /// <param name="username">The username of the employee to search for.</param>
        /// <returns>
        /// The <see cref="Employee"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Employee?> ReadEmployeeAsync(string username);

        /// <summary>
        /// Asynchronously updates an existing employee record in the database.
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> object containing the updated details of the employee.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateEmployeeAsync(Employee employee);
        /// <summary>
        /// Asynchronously deletes an employee record from the database.
        /// </summary>
        /// <param name="id">The ID of the employee to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteEmployeeAsync(int id);
    }
}