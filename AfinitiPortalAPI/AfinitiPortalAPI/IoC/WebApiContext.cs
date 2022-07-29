using AfinitiPortalAPI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI
{
    /// <summary>
    /// This object is filled while authorization process and kept alive during Api-Call operation.
    /// It is used to access authenticated user's data from anywhere.
    /// </summary>
    public class WebApiContext
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string CrowdSSOToken { get; set; }

        public List<ServiceCatalogueRoleDto> ServiceCatalogueRoles { get; set; } = new List<ServiceCatalogueRoleDto>();
    }
}
