using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class SidRpWorkflow1
    {
        public int Id { get; set; }
        public int WorkflowOrder { get; set; }
        public string RegionManager { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
