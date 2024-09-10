using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Domain.Entity.dropdown;
using System.Security.Claims;

namespace smtOffice.Presentation.Controllers
{
    public class LeaveRequestController(ILeaveRequestService leaveRequestService,
        IMapper mapper,
        IDropDownRepository dropDownRepository,
        IEmployeeService employeeService) : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService = leaveRequestService;
        private readonly IMapper _mapper = mapper;
        private readonly IDropDownRepository _dropDownRepository = dropDownRepository;
        private readonly IEmployeeService _employeeService = employeeService;

        /// <summary>
        /// Attempts to retrieve and parse the user's ID from their claims.
        /// </summary>
        /// <param name="myID">The parsed user ID if the retrieval and parsing are successful; otherwise, it is set to 0.</param>
        /// <returns>
        /// <c>true</c> if the user's ID is successfully retrieved and parsed as an integer; otherwise, <c>false</c>.
        /// </returns>
        private bool MyId(out int myID)
        {
            var MyIdd = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            bool isInt = int.TryParse(MyIdd, out myID);

            return isInt;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            int employeeID;
            if(MyId(out employeeID))
            {
                var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync(employeeID);
                return View(leaveRequests);
            }
            return RedirectToAction("Index","Account");
        }
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var absenceReasons = await _dropDownRepository.GetNameFromTableAsync<AbsenceReason>();
            ViewBag.AbsenceReasons = absenceReasons.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();

            var myFullName = User.Claims.Where(c => c.Type == ClaimTypes.Name).Skip(1).FirstOrDefault()?.Value;
            if (myFullName == null )
                return View();
            ViewBag.fullname = myFullName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(LeaveRequestDTO leaveRequestDto)
        {
            if (leaveRequestDto.StartDate >= leaveRequestDto.EndDate)
                ModelState.AddModelError("EndDate", "End Date must be higher than start date");
            if (DateTime.Now.AddDays(-1) > leaveRequestDto.StartDate)
                ModelState.AddModelError("StartDate", "The start date cannot be earlier than today");
            if (ModelState.IsValid)
            {
                int employeeID;
                if (MyId(out employeeID))
                {
                    leaveRequestDto.EmployeeID = employeeID;
                    var employee = await _employeeService.ReadEmployeeAsync(employeeID);
                    var days = (leaveRequestDto.EndDate - leaveRequestDto.StartDate).Days;
                    if(days > employee.OutOfOfficeBalance)
                    {
                        ModelState.AddModelError("EndDate", $"too few days to use. current number of days is{employee.OutOfOfficeBalance}");
                        return View(leaveRequestDto);
                    }
                    await _leaveRequestService.CreateLeaveRequestAsync(leaveRequestDto);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(leaveRequestDto);
        }
        [Authorize]
        public async Task<IActionResult> Details(int leaveRequestID)
        {
            int myId;
            if (!MyId(out myId))
            {
                return RedirectToAction(nameof(Index));
            }
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestID);
            if (leaveRequest == null || leaveRequest.EmployeeID != myId)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int leaveRequestID)
        {
            int myId;
            if (!MyId(out myId))
            {
                return RedirectToAction(nameof(Index));
            }
            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestID);
            if (leaveRequest == null || leaveRequest.EmployeeID != myId)
            {
                return RedirectToAction(nameof(Index));
            }
            await _leaveRequestService.DeleteLeaveRequestAsync(leaveRequestID, myId);
            return RedirectToAction(nameof(Index));
        }
    }
}
