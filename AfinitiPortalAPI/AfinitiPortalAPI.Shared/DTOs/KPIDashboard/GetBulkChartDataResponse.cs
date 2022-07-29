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
    public class GetBulkChartDataResponse : ApiResponse
    {
        [JsonProperty("graphsFixedBy")]
        public string GraphsFixedBy { get; set; }

        [JsonProperty("tableFixedBy")]
        public string TableFixedBy { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("qualityGraphData")]
        public List<GetChartDataResponseItem> QualityData { get; set; } = new List<GetChartDataResponseItem>();

        [JsonProperty("deliverySpeedGraphData")]
        public List<GetChartDataResponseItem> DeliverySpeedData { get; set; } = new List<GetChartDataResponseItem>();

        [JsonProperty("productionQualityGraphData")]
        public List<GetChartDataResponseItem> ProductionQualityData { get; set; } = new List<GetChartDataResponseItem>();

        [JsonProperty("tableData")]
        public GetTableDataResponse TableData { get; set; } = new GetTableDataResponse();
    }
}
