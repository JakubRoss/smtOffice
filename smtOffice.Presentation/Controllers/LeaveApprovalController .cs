using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smtOffice.Application.Interfaces;
using System.Security.Claims;

namespace smtOffice.Presentation.Controllers
{
    public class LeaveApprovalController : Controller
    {
        private readonly ILeaveApprovalCoordinatorService _leaveApprovalCoordinatorService;

        public LeaveApprovalController(ILeaveApprovalCoordinatorService leaveApprovalCoordinatorService)
        {
            _leaveApprovalCoordinatorService = leaveApprovalCoordinatorService;
        }
        [Authorize(Roles = "admin,hrmanager,projectmanager")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Pobierz aktualnego pracownika
                int approverID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Pobierz wnioski o zatwierdzenie z szczegółami
                var model = await _leaveApprovalCoordinatorService.GetApprovalRequestsWithDetailsAsync(approverID);

                return View(model);
            }
            catch (Exception )
            {
                return View();
            }
        }
        [Authorize(Roles = "admin,hrmanager,projectmanager")]
        public async Task<IActionResult> Details(int approvalID)
        {
            try
            {
                // Pobieranie szczegółów wniosku o zatwierdzenie i powiązanych danych
                var model = await _leaveApprovalCoordinatorService.GetApprovalLeaveRequestDetailsAsync(approvalID);

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,hrmanager,projectmanager")]
        public async Task<IActionResult> ProcessRequest(int leaveRequestID, int approverID, string Comment, string action)
        {
            try
            {
                if (action == "Approve")
                {
                    await _leaveApprovalCoordinatorService.ApproveLeaveRequestAsync(leaveRequestID, approverID, Comment);
                    return RedirectToAction(nameof(Index));
                }
                else if (action == "Reject")
                {
                    await _leaveApprovalCoordinatorService.RejectLeaveRequestAsync(leaveRequestID, approverID, Comment);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new InvalidOperationException("Invalid action.");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /ApprovalRequests/Submit/5
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Submit(int leaveRequestID)
        {
            try
            {
                int employeeID = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value);
                await _leaveApprovalCoordinatorService.SubmitRequestAsync(leaveRequestID, employeeID);
                return RedirectToAction(nameof(Details), new { id = leaveRequestID });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
