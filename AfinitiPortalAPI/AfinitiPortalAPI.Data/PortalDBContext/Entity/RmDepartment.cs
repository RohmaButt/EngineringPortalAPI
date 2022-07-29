using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmDepartment
    {
        public RmDepartment()
        {
            RmApprovalWorkflows = new HashSet<RmApprovalWorkflow>();
            RmPhases = new HashSet<RmPhase>();
            RmRegions = new HashSet<RmRegion>();
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
            RmRoles = new HashSet<RmRole>();
            RmSubdepartments = new HashSet<RmSubdepartment>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public string PaycomDepartmentName { get; set; }
        public string PaycomDepartmentId { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RmApprovalWorkflow> RmApprovalWorkflows { get; set; }
        public virtual ICollection<RmPhase> RmPhases { get; set; }
        public virtual ICollection<RmRegion> RmRegions { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
        public virtual ICollection<RmRole> RmRoles { get; set; }
        public virtual ICollection<RmSubdepartment> RmSubdepartments { get; set; }
    }
}
