using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class ServiceCatalogueRoleDto
    {
        public string RoleName { get; set; }

        public string Service { get; set; }

        public string ServiceGroup { get; set; }

        public string Portfolio { get; set; }
    }
}