using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class ServiceCatalogueRole
    {
        public string StaffName { get; set; }
        public string UserKey { get; set; }
        public string LowerUserName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleName { get; set; }
        public string Portfolio { get; set; }
        public string ServiceGroup { get; set; }
        public string Service { get; set; }
    }
}
