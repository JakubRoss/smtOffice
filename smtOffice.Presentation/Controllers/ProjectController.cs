using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces;
using smtOffice.Application.Interfaces.Services;

namespace smtOffice.Presentation.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var projectManager = await _employeeService.ReadEmployeeAsync(project.ProjectManagerID);
            ViewBag.ProjectManager = projectManager.FullName;
            return View(project);
        }

        public async Task<IActionResult> Create()
        {
            var projectmanagers = await _employeeService.ReadEmployeeAsync("projectmanager");

            var viewModel = new ProjectDTO
            {
                projectmanagers = projectmanagers.Select(name => new SelectListItem
                {
                    Value = name.ID.ToString(),
                    Text = name.FullName,
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDTO project)
        {
            if (ModelState.IsValid)
            {
                if(project.StartDate<project.EndDate)
                    ModelState.AddModelError("EndDate", "The end date must be higher than the start date.");
                await _projectService.CreateProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                RedirectToAction(nameof(Index));
            }
            var projectmanagers = await _employeeService.ReadEmployeeAsync("projectmanager");


            project.projectmanagers = projectmanagers.Select(name => new SelectListItem
            {
                Value = name.ID.ToString(),
                Text = name.FullName,
            }).ToList();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectDTO project)
        {
            if (ModelState.IsValid)
            {
                await _projectService.UpdateProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                string errorMessage = string.Empty;
                if (ex.ToString().Contains("REFERENCE"))
                    errorMessage = "Cannot delete. Project is in use";
                else
                    errorMessage = "Niedopracowana db i kaskadowe zachowanie";
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactive(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project != null)
            {
                project.Status = "Inactive";
                await _projectService.UpdateProjectAsync(project);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project != null)
            {
                project.Status = "Active";
                await _projectService.UpdateProjectAsync(project);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToProject(int projectId, List<int> selectedIds)
        {
            if (selectedIds != null && selectedIds.Any())
            {
                await _projectService.AddEmployeesToProject(projectId, selectedIds);
            }

            return RedirectToAction("Index","Employee");
        }
    }
}
