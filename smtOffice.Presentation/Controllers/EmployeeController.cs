using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Domain.Entity.dropdown;

namespace smtOffice.Presentation.Controllers
{
    public class EmployeeController(IEmployeeService employeeService,
        IDropDownRepository dropDownRepository,
        IPasswordHasher passwordHasher) : Controller
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IDropDownRepository _dropDownRepository = dropDownRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<IActionResult> Index()
        {
            var projects = new List<LoginDTO>
            {
                new LoginDTO{ Password = "12", Username="PROJEKT1"},
                new LoginDTO{ Password = "8", Username="PROJEKT_X"},
            };

            var projectSelectList = projects.Select(p => new SelectListItem
            {
                Value = p.Password,
                Text = p.Username
            }).ToList();
            ViewBag.Projects = projectSelectList;

            var employees = await _employeeService.ReadEmployeesAsync();
            var employeesDto = new List<EmployeeDTO>();
            foreach (var employee in employees)
            {
                employee.PasswordHash = string.Empty;
                employee.Photo = null;
                employeesDto.Add(employee);
            }
            return View(employeesDto);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var employee = await _employeeService.ReadEmployeeAsync(id);
                employee.PasswordHash = string.Empty;
                var hrmanager = await _employeeService.ReadEmployeeAsync(employee.PeoplePartnerID);

                if (hrmanager == null)
                {
                    ViewBag.hrmanager = "mango"; 
                }
                else
                {
                    ViewBag.hrmanager = hrmanager.FullName; 
                }
                return View("EmployeeDetails", employee);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Employee");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeDTO employee;
            try
            {
                employee = await _employeeService.ReadEmployeeAsync(id);

                var subdivisions = await _dropDownRepository.GetNameFromTableAsync<Subdivision>();
                var positions = await _dropDownRepository.GetNameFromTableAsync<Position>();
                var hrmanagers = await _employeeService.ReadEmployeeAsync("admin");


                employee.Subdivisions = subdivisions.Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name
                }).ToList();

                employee.Positions = positions.Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name
                }).ToList();
                employee.HRManagers = hrmanagers.Select(n => new SelectListItem
                {
                    Value = n.ID.ToString(),
                    Text = n.FullName
                }).ToList();
                return View(employee);
            }
            catch(Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO.Status != "Active" && employeeDTO.Status != "Inactive")
                    ModelState.AddModelError("Status", "One or more selected statuses are not valid.");

                var currentEmployee = await _employeeService.ReadEmployeeAsync(employeeDTO.ID);
                if (currentEmployee == null)
                    throw new InvalidOperationException("Employee not found");

                if (employeeDTO.Username != currentEmployee.Username)
                {
                    var isTaken = await _employeeService.ReadEmployeeAsync(employeeDTO.Username);
                    if (isTaken != null)
                        ModelState.AddModelError("Username", "Username is taken");
                }

                var validSubdivisions = await _dropDownRepository.GetNameFromTableAsync<Subdivision>();
                var validPositions = await _dropDownRepository.GetNameFromTableAsync<Position>();

                if (employeeDTO.Subdivisions.Any(subdivision => !validSubdivisions.Contains(subdivision.Value)))
                {
                    ModelState.AddModelError("Subdivisions", "One or more selected subdivisions are not valid.");
                }

                if (employeeDTO.Positions.Any(position => !validPositions.Contains(position.Value)))
                {
                    ModelState.AddModelError("Positions", "One or more selected positions are not valid.");
                }

                // Nie nadpisuj hasła i zdjęcia, zachowaj je z obecnych danych pracownika
                employeeDTO.PasswordHash = currentEmployee.PasswordHash;
                employeeDTO.Photo = currentEmployee.Photo;
                employeeDTO.OutOfOfficeBalance = currentEmployee.OutOfOfficeBalance;

                if (ModelState.IsValid)
                {
                    await _employeeService.UpdateEmployeeAsync(employeeDTO);
                    return RedirectToAction(nameof(Details), new { id = employeeDTO.ID });
                }

                return View(employeeDTO);
            }
            catch
            {
                return View(employeeDTO);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
