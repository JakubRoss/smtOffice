using AutoMapper;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces.Repository;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Domain.Entity;

namespace smtOffice.Application.Services
{

    public class ApprovalRequestService(IApprovalRequestRepository approvalRequestRepository, IMapper mapper) : IApprovalRequestService
    {
        private readonly IApprovalRequestRepository _approvalRequestRepository = approvalRequestRepository;

        private readonly IMapper _mapper = mapper;

        public async Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest)
        {
            await _approvalRequestRepository.CreateApprovalRequestAsync(approvalRequest);
        }

        public async Task<ApprovalRequestDTO?> GetApprovalRequestByIdAsync(int id)
        {
            var approvalrequest = await _approvalRequestRepository.GetApprovalRequestByIdAsync(id);
            return _mapper.Map<ApprovalRequestDTO>(approvalrequest);
        }
        public async Task<IEnumerable<ApprovalRequestDTO>> GetAllApprovalRequestsAsync(int approverID)
        {
            var approvalrequests = await _approvalRequestRepository.GetAllApprovalRequestsAsync(approverID);
            if(approvalrequests != null)
            {
                return _mapper.Map<IEnumerable<ApprovalRequestDTO>>(approvalrequests);
            }
            return [];
        }
    }
}
