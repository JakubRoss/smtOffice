namespace smtOffice.Presentation.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using smtOffice.Application.DTOs;
    using System.Security.Claims;
    using smtOffice.Application.Interfaces;
    using smtOffice.Application.Interfaces.Repository;
    using smtOffice.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using smtOffice.Domain.Entity.dropdown;
    using smtoffice.Infrastructure.Repository;

    public class AccountController(IAccountService accountService,
        IEmployeeRepository employeeRepository,
        IEmployeeService employeeService,
        IDropDownRepository dropDownRepository,
        IPasswordHasher passwordHasher) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IDropDownRepository _dropDownRepository = dropDownRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public ActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Details");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Detials");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_accountService.IsValidUser(model).Result)
            {
                var employee = await _employeeRepository.ReadEmployeeAsync(model.Username);
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, employee!.ID.ToString()),
                new Claim(ClaimTypes.Role, employee.Position.ToString()),
                new Claim(ClaimTypes.Name, employee.FullName),
                new Claim("hrmanager",employee.PeoplePartnerID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Details", "Account");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }
        public async Task<IActionResult> Details()
        {
            var id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            var employee = await _employeeService.ReadEmployeeAsync(int.Parse(id));
            employee.ID = default;
            employee.PasswordHash = string.Empty;
            var hrmanager = await _employeeService.ReadEmployeeAsync(employee.PeoplePartnerID);
            ViewBag.hrmanager = hrmanager.FullName;

            return View(employee);
        }
        public async Task<IActionResult> Create()
        {
            var subdivisions = await _dropDownRepository.GetNameFromTableAsync<Subdivision>();
            var positions = await _dropDownRepository.GetNameFromTableAsync<Position>();
            var hrmanagers = await _employeeService.ReadEmployeeAsync("admin");

            var viewModel = new EmployeeDTO
            {
                Subdivisions = subdivisions.Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name
                }).ToList(),

                Positions = positions.Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name
                }).ToList(),
                HRManagers = hrmanagers.Select(n => new SelectListItem
                {
                    Value = n.ID.ToString(),
                    Text = n.FullName
                }).ToList(),

            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDTO employeeDTO)
        {
            employeeDTO.Status = "Active";
            ModelState["Status"]!.ValidationState = ModelValidationState.Valid;

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

            if (ModelState.IsValid)
            {
                try
                {
                    employeeDTO.PasswordHash = _passwordHasher.HashPassword(employeeDTO.PasswordHash);
                    employeeDTO.OutOfOfficeBalance = 20;
                    employeeDTO.Photo = null;
                    await _employeeService.CreateEmployeeAsync(employeeDTO);
                    return RedirectToAction("Index","Employee");
                }

                catch
                {
                    return View(employeeDTO);
                }
            }

            return View(employeeDTO);

        }
    }

}
