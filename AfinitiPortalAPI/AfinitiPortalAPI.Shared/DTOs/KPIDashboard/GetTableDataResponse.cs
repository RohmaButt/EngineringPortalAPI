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
    public class GetTableDataResponse : ApiResponse
    {
        [JsonProperty("deliverySpeedTargetScore")]
        public double? DeliverySpeedTargetScore { get; set; }
        
        [JsonProperty("qualityTargetScore")]
        public double? QualityTargetScore { get; set; }

        [JsonProperty("productionQualityTargetScore")]
        public double? ProductionQualityTargetScore { get; set; }

        [JsonProperty("data")]
        public List<GetTableDataResponseItem> Data { get; set; } = new List<GetTableDataResponseItem>();
    }

    public class GetTableDataResponseItem
    {
        [JsonProperty("filterTerm")]
        public string FilterTerm { get; set; }

        [JsonProperty("fixedValue")]
        public string FixedValue { get; set; }

        [JsonProperty("qualityMetric")]
        public decimal QualityMetric { get; set; }

        [JsonProperty("deliverySpeedMetric")]
        public decimal DeliverySpeedMetric { get; set; }
        
        [JsonProperty("productionQualityMetric")]
        public decimal ProductionQualityMetric { get; set; }
    }
}