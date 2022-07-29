using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmTeam
    {
        public RmTeam()
        {
            RmApprovalWorkflows = new HashSet<RmApprovalWorkflow>();
            RmRegions = new HashSet<RmRegion>();
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
            RmRoles = new HashSet<RmRole>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public int SubdepartmentId { get; set; }
        public bool IsActive { get; set; }

        public virtual RmSubdepartment Subdepartment { get; set; }
        public virtual ICollection<RmApprovalWorkflow> RmApprovalWorkflows { get; set; }
        public virtual ICollection<RmRegion> RmRegions { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
        public virtual ICollection<RmRole> RmRoles { get; set; }
    }
}
