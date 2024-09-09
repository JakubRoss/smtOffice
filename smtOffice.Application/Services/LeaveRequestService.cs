using AutoMapper;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Domain.Entity;

namespace smtOffice.Application.Services
{
    public class LeaveRequestService(ILeaveRequestRepository leaveRequestRepository,
        IMapper mapper) :ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
        private readonly IMapper _mapper = mapper;

        public async Task CreateLeaveRequestAsync(LeaveRequestDTO leaveRequestDTO)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(leaveRequestDTO);

                leaveRequestDTO.Status = "New";
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDTO);
                await _leaveRequestRepository.CreateLeaveRequestAsync(leaveRequest);
            }
            catch (Exception) { }
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequestsAsync(int employeeID)
        {
            var leaverequests = await _leaveRequestRepository.ReadLeaveRequestsAsync(employeeID);
            if(leaverequests != null)
            {
                var leaveRequestsDTO = _mapper.Map<IEnumerable<LeaveRequestDTO>>(leaverequests);
                return leaveRequestsDTO;
            }
            return [];
        }

        public async Task<LeaveRequestDTO?> GetLeaveRequestByIdAsync(int leaveRequestID)
        {
            try
            {
                if (leaveRequestID <= 0)
                    throw new ArgumentOutOfRangeException(nameof(leaveRequestID));

                var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(leaveRequestID) ?? throw new ArgumentNullException(nameof(leaveRequestID));
                return _mapper.Map<LeaveRequestDTO>(leaveRequest);
            }
            catch (Exception) 
            {
                return null;
            }
        }

        public async Task DeleteLeaveRequestAsync(int leaveRequestID, int employeeID)
        {
            if (leaveRequestID <= 0)
                throw new ArgumentOutOfRangeException(nameof(leaveRequestID));
            var leaveRequest = await _leaveRequestRepository.ReadLeaveRequestAsync(leaveRequestID);
            if (leaveRequest != null && leaveRequest.EmployeeID == employeeID && leaveRequest.Status == "New")
                await _leaveRequestRepository.DeleteLeaveRequestAsync(leaveRequestID);
        }
    }
}
