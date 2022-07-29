using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmEmployeeRegionalMapping
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int RegionId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }

        public virtual RmRegion Region { get; set; }
    }
}
