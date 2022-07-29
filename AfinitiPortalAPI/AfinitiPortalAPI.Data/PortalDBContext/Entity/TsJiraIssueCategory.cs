using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class TsJiraIssueCategory
    {
        public TsJiraIssueCategory()
        {
            TsJiraIssueTypes = new HashSet<TsJiraIssueType>();
            TsWorklogs = new HashSet<TsWorklog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? HermesAccount { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<TsJiraIssueType> TsJiraIssueTypes { get; set; }
        public virtual ICollection<TsWorklog> TsWorklogs { get; set; }
    }
}
