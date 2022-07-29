using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared.DTOs.KPIDashboard
{
    public class OKRTableItem
    {
        public string Portfolio { get; set; }

        public string PortfolioOkrDesc { get; set; }

        public string PortfolioOkrIssueLink { get; set; }

        public  List<OKRServiceGroupTableItem> ServiceGroupOkrs { get; set; }
    }

    public class OKRServiceGroupTableItem
    {
        public string ServiceGroup { get; set; }

        public string ServiceGroupOkrDesc { get; set; }

        public string ServiceGroupOkrIssueLink { get; set; }

        public List<OKRServiceTableItem> ServiceOkrs { get; set; }
    }

    public class OKRServiceTableItem
    {
        public string Service { get; set; }

        public string ServiceOkrDesc { get; set; }

        public string ServiceOkrIssueLink { get; set; }
    }
}