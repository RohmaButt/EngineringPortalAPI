using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesQueue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public int? Positions { get; set; }
        public int? OptimizationType { get; set; }
        public int? UsdMonthlyRevenue { get; set; }
        public bool? Deleted { get; set; }
        public int? CallVolume { get; set; }
        public int? OnPercentage { get; set; }
        public string OptimizationDescriptionDetails { get; set; }
        public int? FixedFeeAmount { get; set; }
        public bool? SwitchDistributionSetup { get; set; }
        public string PilotStrategicRationale { get; set; }
        public int? Cc2ApplicationId { get; set; }
        public bool? Pilot { get; set; }
        public int? CallType { get; set; }
        public int? OptimizationDescription { get; set; }
        public int? PricingCategorization { get; set; }
        public int? PricingModel { get; set; }
        public bool? MinimumFee { get; set; }
        public int? MinimumFeeAmount { get; set; }
        public bool? FullDataAccess { get; set; }
        public string FullDataAccessDetails { get; set; }
        public int? AccountId { get; set; }
    }
}
