using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class SidRpRegion
    {
        public int Id { get; set; }
        public string Sidregion { get; set; }
        public bool IsRegion { get; set; }
        public string RegionManager { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public int? SidregionId { get; set; }
    }
}
