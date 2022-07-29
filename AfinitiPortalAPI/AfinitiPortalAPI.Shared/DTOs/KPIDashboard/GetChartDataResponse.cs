using AfinitiPortalAPI.Shared.Enums;
using AfinitiPortalAPI.Shared.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class GetChartDataResponse : ApiResponse
    {
        [JsonProperty("fixedBy")]
        public string FixedBy { get; set; }

        [JsonProperty("data")]
        public List<GetChartDataResponseItem> Data { get; set; } = new List<GetChartDataResponseItem>();
    }

    public class GetChartDataResponseMetricItem
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("metricValue")]
        public decimal MetricValue { get; set; }
    }

    public class GetChartDataResponseItem
    {
        [JsonProperty("fixedValue")]
        public string FixedValue { get; set; }

        [JsonProperty("metrics")]
        public List<GetChartDataResponseMetricItem> Metrics { get; set; } = new List<GetChartDataResponseMetricItem>();
    }
}
