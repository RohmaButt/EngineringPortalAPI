using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesAccountPhase
    {
        public int Id { get; set; }
        public string Enum { get; set; }
        public int? Value { get; set; }
        public string Description { get; set; }
    }
}
