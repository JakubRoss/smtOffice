using smtOffice.Domain.Entity;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.Interfaces;
using smtOffice.Application.DTOs;
using AutoMapper;
using System.Reflection;

namespace smtOffice.Application.Services
{
    internal class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task CreateProjectAsync(ProjectDTO project)
        {
            var projectDTO = _mapper.Map<Project>(project);
            if(projectDTO != null)
                await _projectRepository.CreateProjectAsync(projectDTO);
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.ReadProjectAsync(id); ;
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.ReadProjectsAsync();
            return _mapper.Map<List<ProjectDTO>>(projects);
        }

        public async Task UpdateProjectAsync(ProjectDTO projectDto)
        {
            // Pobierz aktualny projekt z repozytorium
            var currentProject = await _projectRepository.ReadProjectAsync(projectDto.ID);

            if (currentProject == null)
                throw new ArgumentNullException(nameof(currentProject));

            // Pobierz właściwości z obiektu ProjectDTO
            var projectProperties = typeof(ProjectDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in projectProperties)
            {
                if (property.Name == "projectmanagers")
                {
                    continue;
                }

                // Pobierz wartość właściwości z DTO
                var value = property.GetValue(projectDto);

                // Sprawdź, czy wartość nie jest null, pustym ciągiem lub białymi znakami
                if (value != null && !(value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    // Znajdź odpowiednią właściwość w currentProject
                    var currentProjectProperty = typeof(Project).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                    // Upewnij się, że właściwość istnieje i jest zapisywalna
                    if (currentProjectProperty != null && currentProjectProperty.CanWrite)
                    {
                        // Ustaw wartość z DTO w obiekcie currentProject
                        currentProjectProperty.SetValue(currentProject, value);
                    }
                }
            }

            // Zapisz zmiany przez repozytorium
            await _projectRepository.UpdateProjectAsync(currentProject);
        }


        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteProjectAsync(id);
        }

        public async Task AddEmployeesToProject(int projectId, List<int> selectedIds)
        {
            var project = _projectRepository.ReadProjectAsync(projectId);
            if (project != null)
            {
                await _projectRepository.AddEmployeesToProjectAsync(projectId, selectedIds);
            }
        }
    }
}
