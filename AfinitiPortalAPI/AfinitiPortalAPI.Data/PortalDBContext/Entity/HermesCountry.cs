using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesCountry
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public int DefaultCurrency { get; set; }
        public int RegionId { get; set; }
    }
}
