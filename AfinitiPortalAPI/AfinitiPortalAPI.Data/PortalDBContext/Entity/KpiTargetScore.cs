using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class KpiTargetScore
    {
        public long Id { get; set; }
        public string Quarter { get; set; }
        public float? DeliverySpeed { get; set; }
        public float? Quality { get; set; }
        public float? ProductionQuality { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
