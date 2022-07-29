using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesSwitchPlatform
    {
        public int Id { get; set; }
        public int? ProviderId { get; set; }
        public string Name { get; set; }
    }
}
