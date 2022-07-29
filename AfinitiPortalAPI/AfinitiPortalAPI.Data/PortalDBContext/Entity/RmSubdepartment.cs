using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmSubdepartment
    {
        public RmSubdepartment()
        {
            RmApprovalWorkflows = new HashSet<RmApprovalWorkflow>();
            RmRegions = new HashSet<RmRegion>();
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
            RmRoles = new HashSet<RmRole>();
            RmTeams = new HashSet<RmTeam>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string PaycomSubdepartmentName { get; set; }
        public string PaycomSubdepartmentId { get; set; }
        public bool IsActive { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual ICollection<RmApprovalWorkflow> RmApprovalWorkflows { get; set; }
        public virtual ICollection<RmRegion> RmRegions { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
        public virtual ICollection<RmRole> RmRoles { get; set; }
        public virtual ICollection<RmTeam> RmTeams { get; set; }
    }
}
