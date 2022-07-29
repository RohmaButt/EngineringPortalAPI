using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.DTOs.KPIDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Data.Extensions
{
    public static class DomainModelExtensions
    {
        public static List<OKRTableItem> ToOkrTableItemForService(this IEnumerable<PortalOkrBackend> input)
        {
            return input.GroupBy(x => new { x.Portfolio, x.PortfolioOkr, x.PortfolioOkrIssueLink }).Select(x => new OKRTableItem()
            {
                Portfolio = x.Key.Portfolio,
                PortfolioOkrDesc = x.Key.PortfolioOkr,
                PortfolioOkrIssueLink = x.Key.PortfolioOkrIssueLink,
                ServiceGroupOkrs = x.GroupBy(y => new { y.ServiceGroup, y.ServiceGroupOkr, y.ServiceGroupOkrIssueLink }).Select(o => new OKRServiceGroupTableItem()
                {
                    ServiceGroup = o.Key.ServiceGroup,
                    ServiceGroupOkrDesc = o.Key.ServiceGroupOkr,
                    ServiceGroupOkrIssueLink = o.Key.ServiceGroupOkrIssueLink,
                    ServiceOkrs = o.GroupBy(h => new { h.Service, h.ServiceOkr, h.ServiceOkrIssueLink }).Select(p => new OKRServiceTableItem()
                    {
                        Service = p.Key.Service,
                        ServiceOkrDesc = p.Key.ServiceOkr,
                        ServiceOkrIssueLink = p.Key.ServiceOkrIssueLink
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public static List<OKRTableItem> ToOkrTableItemForServiceGroup(this IEnumerable<PortalOkrBackend> input)
        {
            return input.GroupBy(x => new { x.Portfolio, x.PortfolioOkr, x.PortfolioOkrIssueLink }).Select(x => new OKRTableItem()
            {
                Portfolio = x.Key.Portfolio,
                PortfolioOkrDesc = x.Key.PortfolioOkr,
                PortfolioOkrIssueLink = x.Key.PortfolioOkrIssueLink,
                ServiceGroupOkrs = x.GroupBy(y => new { y.ServiceGroup, y.ServiceGroupOkr, y.ServiceGroupOkrIssueLink }).Select(y => new OKRServiceGroupTableItem()
                {
                    ServiceGroup = y.Key.ServiceGroup,
                    ServiceGroupOkrDesc = y.Key.ServiceGroupOkr,
                    ServiceGroupOkrIssueLink = y.Key.ServiceGroupOkrIssueLink
                }).ToList()
            }).ToList();
        }

        public static List<OKRTableItem> ToOkrTableItemForPortfolio(this IEnumerable<PortalOkrBackend> input)
        {
            return input.GroupBy(x => new { x.Portfolio, x.PortfolioOkr, x.PortfolioOkrIssueLink }).Select(x => new OKRTableItem()
            {
                Portfolio = x.Key.Portfolio,
                PortfolioOkrDesc = x.Key.PortfolioOkr,
                PortfolioOkrIssueLink = x.Key.PortfolioOkrIssueLink
            }).ToList();
        }
    }
}
