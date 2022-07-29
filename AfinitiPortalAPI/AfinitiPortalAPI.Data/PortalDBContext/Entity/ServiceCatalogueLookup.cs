using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class ServiceCatalogueLookup
    {
        public string Portfolio { get; set; }
        public string PortfolioUrl { get; set; }
        public string ServiceGroup { get; set; }
        public string ServiceGroupUrl { get; set; }
        public string Service { get; set; }
        public string ServiceUrl { get; set; }
    }
}
