using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmPlanning
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string EmployeeEmail { get; set; }
        public string PlanningCycle { get; set; }
        public int? AssignedAccountId { get; set; }
        public int? AssignedQueueId { get; set; }
        public int? AssignedRoleId { get; set; }
        public float? Assignment { get; set; }
        public string DataEntryBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? Status { get; set; }

        public virtual RmRole AssignedRole { get; set; }
    }
}
