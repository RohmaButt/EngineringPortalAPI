using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class PortalOkrBackend
    {
        public string Interval { get; set; }
        public string Portfolio { get; set; }
        public string PortfolioOkr { get; set; }
        public string ServiceGroup { get; set; }
        public string ServiceGroupOkr { get; set; }
        public string Service { get; set; }
        public string ServiceOkr { get; set; }
        public string OkrLevel { get; set; }
        public string PortfolioOkrIssueLink { get; set; }
        public string ServiceGroupOkrIssueLink { get; set; }
        public string ServiceOkrIssueLink { get; set; }
    }
}
