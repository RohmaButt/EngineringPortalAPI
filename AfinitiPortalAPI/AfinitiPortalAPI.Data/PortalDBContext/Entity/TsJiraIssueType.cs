using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class TsJiraIssueType
    {
        public TsJiraIssueType()
        {
            TsWorklogs = new HashSet<TsWorklog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? IssueCategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual TsJiraIssueCategory IssueCategory { get; set; }
        public virtual ICollection<TsWorklog> TsWorklogs { get; set; }
    }
}
