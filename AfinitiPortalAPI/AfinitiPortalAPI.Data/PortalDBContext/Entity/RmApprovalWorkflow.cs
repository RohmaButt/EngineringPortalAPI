using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmApprovalWorkflow
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubdepartmentId { get; set; }
        public int? TeamId { get; set; }
        public int WorkflowOrder { get; set; }
        public string ApproverEmail { get; set; }
        public bool IsActive { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual RmSubdepartment Subdepartment { get; set; }
        public virtual RmTeam Team { get; set; }
    }
}
