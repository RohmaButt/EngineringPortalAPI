using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class KpiBackendPortal
    {
        public string Interval { get; set; }
        public string Service { get; set; }
        public string ServiceGroup { get; set; }
        public string Portfolio { get; set; }
        public int? Week { get; set; }
        public DateTime? StartOfWeekDate { get; set; }
        public DateTime? EndOfWeekDate { get; set; }
        public float? DeliverySpeedIntervalSeOld { get; set; }
        public float? DeliverySpeedSgL6wAvgOld { get; set; }
        public float? DeliverySpeedIntervalSgOld { get; set; }
        public float? DeliverySpeedPoL6wAvgOld { get; set; }
        public float? QualityIntervalSe { get; set; }
        public float? QualitySgL6wAvg { get; set; }
        public float? QualityIntervalSg { get; set; }
        public float? QualityPoL6wAvg { get; set; }
        public float? ProdQualityIntervalSeOld { get; set; }
        public float? ProdQualitySgL6wAvgOld { get; set; }
        public float? ProdQualityIntervalSgOld { get; set; }
        public float? ProdQualityPoL6wAvgOld { get; set; }
        public float? DeliverySpeedSpIntervalSe { get; set; }
        public float? DeliverySpeedSpSgL6wAvg { get; set; }
        public float? DeliverySpeedSpIntervalSg { get; set; }
        public float? DeliverySpeedSpPoL6wAvg { get; set; }
        public float? QaLeadTimeIntervalSe { get; set; }
        public float? QaLeadTimeSgL6wAvg { get; set; }
        public float? QaLeadTimeIntervalSg { get; set; }
        public float? QaLeadTimePoL6wAvg { get; set; }
        public float? DeliverySpeedIntervalSe { get; set; }
        public float? DeliverySpeedSgL6wAvg { get; set; }
        public float? DeliverySpeedIntervalSg { get; set; }
        public float? DeliverySpeedPoL6wAvg { get; set; }
        public float? ProdQualityIntervalSe { get; set; }
        public float? ProdQualitySgL6wAvg { get; set; }
        public float? ProdQualityIntervalSg { get; set; }
        public float? ProdQualityPoL6wAvg { get; set; }
    }
}
