using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs.KPIDashboard
{
    public class ChartDataDto
    {
        public DateTime? IssueCreationDate { get; set; }

        public DateTime? IssueResolutionDate { get; set; }

        public decimal? IssueId { get; set; }

        public string Portfolio { get; set; }

        public string ServiceGroup { get; set; }

        public string Service { get; set; }

        public decimal? MetricNo1Dividend { get; set; }

        public decimal? MetricNo1Divisor { get; set; }

        public decimal? MetricNo9Dividend { get; set; }

        public decimal? MetricNo15Dividend { get; set; }

        public decimal? MetricNo15Divisor { get; set; }

        public string StaffName { get; set; }

        public DateTime? IssueCreationDay { get; set; }

        public double? SharedCoefService { get; set; }

        // For Projections...
        public decimal? CoefAvg { get; set; }

        public decimal? DivisorValue { get; set; }

        public decimal? DividendValue { get; set; }

        // For Delivery Speed...
        public string CalanderDay { get; set; }

        public decimal NetHeadCount { get; set; }

        public string Interval { get; set; }
    }
}
