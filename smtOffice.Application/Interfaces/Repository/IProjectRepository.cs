using smtOffice.Domain.Entity;

namespace smtoffice.Infrastructure.Repository
{
    public interface IProjectRepository
    {   /// <summary>
        /// Inserts a new project into the database.
        /// </summary>
        /// <param name="project">The <see cref="Project"/> object to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateProjectAsync(Project project);
        /// <summary>
        /// Deletes a project from the database based on the provided identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteProjectAsync(int id);
        /// <summary>
        /// Retrieves all projects from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{Project}"/> of all projects.</returns>
        Task<IEnumerable<Project>> ReadProjectsAsync();
        /// <summary>
        /// Retrieves a project from the database based on the provided identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Project"/> if found; otherwise, null.</returns>
        Task<Project?> ReadProjectAsync(int id);
        /// <summary>
        /// Updates an existing project in the database.
        /// </summary>
        /// <param name="project">The <see cref="Project"/> object with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateProjectAsync(Project project);
        /// <summary>
        /// Assigns a specified project to a list of employees by updating the ProjectID for each employee.
        /// </summary>
        /// <param name="projectId">The ID of the project to which the employees will be assigned.</param>
        /// <param name="employeeIds">A list of employee IDs that need to be updated with the specified ProjectID.</param>
        /// <returns>
        /// A task representing the asynchronous operation of updating the employees' ProjectID in the database.
        /// </returns>
        Task AddEmployeesToProjectAsync (int projectId, List<int> employeeIds);
    }
}