using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;

namespace AfinitiPortalAPI.Profiles
{
    public class ResourceModelProfile : Profile
    {
        public ResourceModelProfile()
        {
            CreateMap<ResourceModelRolesGroupDTO, ResourceModelRolesGroup>().ReverseMap();
            CreateMap<RmRegionDTO, RmRegion>().ReverseMap();
            CreateMap<RmProductDTO, RmProduct>().ReverseMap();
            CreateMap<RmRevenueBracketDTO, RmRevenueBracket>().ReverseMap();
            CreateMap<RmAccountRegionalMappingDTO, RmAccountRegionalMapping>().ReverseMap();
            CreateMap<RmEmployeeRegionalMappingDTO, RmEmployeeRegionalMapping>().ReverseMap();
            CreateMap<RmRoleDTO, RmRole>().ReverseMap();
            CreateMap<RmRoleDTOTrimmed, RmRole>().ReverseMap();
            CreateMap<RmEmployeeRoleMappingDTO, RmEmployeeRoleMapping>().ReverseMap();
            CreateMap<RmDepartmentDTO, RmDepartment>().ReverseMap();
            CreateMap<RmSubdepartmentDTO, RmSubdepartment>().ReverseMap();
            CreateMap<RmTeamDTO, RmTeam>().ReverseMap();
            CreateMap<EmployeeSwitchKnowledgeDTO, RmEmployeeSwitchPlatformKnowledge>().ReverseMap();
            CreateMap<EmployeeSwitchKnowledgeTrimmedDTO, RmEmployeeSwitchPlatformKnowledge>().ReverseMap();
        }
    }
}

