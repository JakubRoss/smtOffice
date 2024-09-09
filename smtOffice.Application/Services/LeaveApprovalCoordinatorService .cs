using smtOffice.Application.Interfaces.Repository;
using smtOffice.Domain.Entity;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.Interfaces;
using smtOffice.Application.DTOs;
using AutoMapper;

namespace smtOffice.Application.Services
{
    internal class LeaveApprovalCoordinatorService(ILeaveRequestRepository leaveRequestRepository,
                                           IApprovalRequestRepository approvalRequestRepository,
                                           IEmployeeRepository employeeRepository,
                                           IProjectRepository projectRepository,
                                           IMapper mapper) : ILeaveApprovalCoordinatorService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
        private readonly IApprovalRequestRepository _approvalRequestRepository = approvalRequestRepository;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IMapper _mapper = mapper;

        public async Task ApproveLeaveRequestAsync(int leaveRequestID, int approverID, string approvalComment)
        {
            // Fetch leave request
            var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(leaveRequestID);
            if (leaveRequest == null)
                throw new InvalidOperationException("Leave request not found.");

            // Fetch approval request
            var approvalRequest = await _approvalRequestRepository.GetApprovalRequestByLeaveRequestAsync(leaveRequestID,approverID);
            if (approvalRequest == null || approvalRequest.Status != "New")
                throw new InvalidOperationException("Approval request not found.");

            // Check if approverID is authorized
            if (approvalRequest.ApproverID != approverID)
                throw new InvalidOperationException("Unauthorized approver.");

            var employee = await _employeeRepository.ReadEmployeeAsync(leaveRequest.EmployeeID);
            if (employee == null)
                throw new InvalidOperationException("Unauthorized approver.");
            employee.OutOfOfficeBalance = (leaveRequest.EndDate - leaveRequest.StartDate).Days;
            await _employeeRepository.UpdateEmployeeAsync(employee);

            // Update approval status
            approvalRequest.Status = "Approved";
            approvalRequest.Comment = approvalComment;
            await _approvalRequestRepository.UpdateApprovalRequestAsync(approvalRequest);

            // Update leave request status
            leaveRequest.Status = "Approved";
            leaveRequest.Comment = approvalComment;
            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);


        }

        public async Task RejectLeaveRequestAsync(int leaveRequestID, int approverID, string rejectionComment)
        {
            // Fetch leave request
            var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(leaveRequestID);
            if (leaveRequest == null)
                throw new InvalidOperationException("Leave request not found.");

            // Fetch approval request
            var approvalRequest = await _approvalRequestRepository.GetApprovalRequestByLeaveRequestAsync(leaveRequestID);
            if (approvalRequest == null || approvalRequest.Status != "New")
                throw new InvalidOperationException("Approval request not found.");

            // Check if approverID is authorized
            if (approvalRequest.ApproverID != approverID)
                throw new InvalidOperationException("Unauthorized approver.");

            // Update approval status
            approvalRequest.Status = "Rejected";
            approvalRequest.Comment = rejectionComment;
            await _approvalRequestRepository.UpdateApprovalRequestAsync(approvalRequest);

            // Update leave request status
            leaveRequest.Status = "Rejected";
            leaveRequest.Comment = rejectionComment;
            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
        }

        public async Task SubmitRequestAsync(int leaveRequestID, int employeeID)
        {
            if (leaveRequestID <= 0)
                throw new ArgumentOutOfRangeException(nameof(leaveRequestID));


            var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(leaveRequestID);
            if (leaveRequest == null || leaveRequest.EmployeeID != employeeID || leaveRequest.Status != "New")
                throw new InvalidOperationException("Invalid leave request submission.");


            var employee = await _employeeRepository.ReadEmployeeAsync(employeeID);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));


            leaveRequest.Status = "Submitted";
            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

            var approvalRequests = new ApprovalRequest();
            approvalRequests.Status = "New";
            approvalRequests.LeaveRequestID = leaveRequestID;


            if (employee.ProjectID != null)
            {
                var project = await _projectRepository.ReadProjectAsync(employee.ProjectID.Value);
                if (project?.ProjectManagerID != null && project.ProjectManagerID != employee.PeoplePartnerID)
                {
                    approvalRequests.ApproverID = project.ProjectManagerID;

                    await _approvalRequestRepository.CreateApprovalRequestAsync(approvalRequests);
                }
            }

            approvalRequests.ApproverID = employee.PeoplePartnerID;
            await _approvalRequestRepository.CreateApprovalRequestAsync(approvalRequests);
            
        }

        public async Task<IEnumerable<ApprovalLeaveRequestDTO>> GetApprovalRequestsWithDetailsAsync(int approverID)
        {
            // Pobierz wszystkie wnioski o zatwierdzenie dla podanego menedżera
            var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsAsync(approverID);

            // Pobierz szczegóły wniosków urlopowych
            var leaveRequests = new List<ApprovalLeaveRequestDTO>();

            foreach (var approvalRequest in approvalRequests)
            {
                var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(approvalRequest.LeaveRequestID);
                if (leaveRequest != null)
                {
                    var employee = await _employeeRepository.ReadEmployeeAsync(leaveRequest.EmployeeID);

                    // Mapuj obiekty domenowe na DTO
                    var leaveRequestDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);
                    var approvalRequestDTO = _mapper.Map<ApprovalRequestDTO>(approvalRequest);

                    leaveRequests.Add(new ApprovalLeaveRequestDTO
                    {
                        ApprovalRequest = approvalRequestDTO,
                        LeaveRequest = leaveRequestDTO,
                        FullName = employee.FullName
                    });
                }
            }

            return leaveRequests;

        }
        public async Task<ApprovalLeaveRequestDTO> GetApprovalLeaveRequestDetailsAsync(int approvalID)
        {
            var approvalRequest = await _approvalRequestRepository.GetApprovalRequestByIdAsync(approvalID);
            if (approvalRequest == null)
            {
                throw new InvalidOperationException("Approval request not found.");
            }

            var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(approvalRequest.LeaveRequestID);
            if (leaveRequest == null)
            {
                throw new InvalidOperationException("Leave request not found.");
            }

            var employee = await _employeeRepository.ReadEmployeeAsync(leaveRequest.EmployeeID);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            var approvalRequestDTO = _mapper.Map<ApprovalRequestDTO>(approvalRequest);
            var leaveRequestDTO = _mapper.Map<LeaveRequestDTO>(leaveRequest);

            var model = new ApprovalLeaveRequestDTO
            {
                ApprovalRequest = approvalRequestDTO,
                LeaveRequest = leaveRequestDTO,
                FullName = employee.FullName,
            };

            return model;
        }
    }
}
