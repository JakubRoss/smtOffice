using smtOffice.Application.DTOs;
using smtOffice.Domain.Entity;

namespace smtOffice.Application.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">The project to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateProjectAsync(ProjectDTO project);
        /// <summary>
        /// Deletes a project by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteProjectAsync(int id);
        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{Project}"/> of all projects.</returns>
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
        /// <summary>
        /// Retrieves a project by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="ProjectDTO"/> if found; otherwise, null.</returns>
        Task<ProjectDTO?> GetProjectByIdAsync(int id);
        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="project">The project with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateProjectAsync(ProjectDTO project);
        /// <summary>
        /// Assigns a specified project to a list of employees by updating the ProjectID for each employee.
        /// </summary>
        /// <param name="projectId">The ID of the project to which the employees will be assigned.</param>
        /// <param name="employeeIds">A list of employee IDs that need to be updated with the specified ProjectID.</param>
        /// <returns>
        /// A task representing the asynchronous operation of updating the employees' ProjectID in the database.
        /// </returns>
        Task AddEmployeesToProject(int projectId, List<int> selectedIds);
    }
}