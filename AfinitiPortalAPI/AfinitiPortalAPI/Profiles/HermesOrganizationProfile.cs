using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Profiles
{
    public class HermesOrganizationProfile : Profile
    {
        public HermesOrganizationProfile()
        {
            CreateMap<SwitchProvidersDTO, HermesOrganization>()
                .ReverseMap();
        }
    }
}
