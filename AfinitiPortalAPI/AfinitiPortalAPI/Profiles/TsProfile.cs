using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;

namespace AfinitiPortalAPI.Profiles
{
    public class TsProfile : Profile
    {
        public TsProfile()
        {
            CreateMap<TsPeriodDTO, TsPeriod>().ReverseMap();
            CreateMap<TsWorklogDTO, TsWorklog>().ReverseMap();
            CreateMap<TsWorklogTrimmedDTO, TsWorklog>().ReverseMap();
            CreateMap<TsJiraIssueCategoryDTO, TsJiraIssueCategory>().ReverseMap();
            CreateMap<TsJiraIssueTypeDTO, TsJiraIssueType>().ReverseMap();
            CreateMap<TsJiraIssueTypeDTO, HermesOrganization>().ReverseMap();
            CreateMap<TsWorklogAdminDTO, TsWorklog>().ReverseMap();
        }
    }
}

