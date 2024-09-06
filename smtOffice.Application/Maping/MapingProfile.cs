using AutoMapper;
using smtOffice.Application.DTOs;
using smtOffice.Domain.Entity;

namespace smtOffice.Application.Maping
{
    public class MapingProfile : Profile
    {
        public MapingProfile()
        {
            CreateMap<Employee,EmployeeDTO>()
                .ForMember(i => i.Subdivisions, opt => opt.Ignore())
                .ForMember(i => i.Positions, opt => opt.Ignore())
                .ForMember(i => i.AbsenceReasons, opt => opt.Ignore())
                .ForMember(i => i.HRManagers, opt => opt.Ignore());
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(i => i.ID, opt => opt.Ignore());

            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>()
                .ForMember(i => i.ID, opt => opt.Ignore());
        }
    }
}
