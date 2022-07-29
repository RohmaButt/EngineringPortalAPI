using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class TsWorklog
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public string Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public float LoggedHoursPercent { get; set; }
        public string Description { get; set; }
        public sbyte? ApprovalStatus { get; set; }
        public string ApproverEmail { get; set; }
        public DateTime InsertionDate { get; set; }
        public string InsertionUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifierName { get; set; }
        public bool IsActive { get; set; }

        public virtual TsJiraIssueCategory Category { get; set; }
        public virtual TsJiraIssueType Type { get; set; }
    }
}
