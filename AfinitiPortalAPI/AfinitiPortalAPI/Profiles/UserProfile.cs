using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;

namespace AfinitiPortalAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<PaycomEmployeeFull_DTO, PaycomEmployeeFull>()
                .ReverseMap();

            CreateMap<AfinitiUser, PaycomEmployeeFull>()
                .ReverseMap();

            CreateMap<PaycomDepartments, PaycomEmployeeFull>()
                .ReverseMap();

            CreateMap<EmployeeHeads_DTO, PaycomEmployeeFull>()
                .ReverseMap();

            CreateMap<NewEmployeeSwitchKnowledgeDTO, PaycomEmployeeFull>()
                .ReverseMap();
            
            CreateMap<PaycomEmployeeFull, PaycomEmployeeFull_DTOFlat>()
                .ForMember(flat => flat.nodeId, options => options.MapFrom(src => src.EmployeeCode))
                .ForMember(flat => flat.name, options => options.MapFrom(src => src.EmployeeName))
                .ForMember(flat => flat.parentNodeId, options => options.MapFrom(src => src.FirstSupervisorCode))
                .ForMember(flat => flat.positionName, options => options.MapFrom(src => src.PositionTitle))
                //.ForMember(flat => flat.Location, options => options.MapFrom(src => src.LocationRaw))
                .ForMember(flat => flat.UserName, options => options.MapFrom(src => src.NtLogin))
                .ReverseMap();
        }
    }
}
