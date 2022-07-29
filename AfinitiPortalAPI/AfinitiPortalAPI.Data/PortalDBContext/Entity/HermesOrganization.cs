using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public int? Currency { get; set; }
        public int? EnterpriseDealFixedFeeAmount { get; set; }
        public int? GroupId { get; set; }
        public ulong? Inactive { get; set; }
        public int? Industry { get; set; }
        public int? PrimarySwitchProviderId { get; set; }
        public int? RevenuePotential { get; set; }
        public string Discriminator { get; set; }
        public int? Cc1ApplicationId { get; set; }
        public ulong? ClientAgreedToExpand { get; set; }
        public ulong? AllQueuesDeployed { get; set; }
        public int? SecondarySwitchProviderId { get; set; }
        public int? PrimarySwitchPlatformId { get; set; }
        public int? SecondarySwitchPlatformId { get; set; }
        public ulong Deleted { get; set; }
        public string Code { get; set; }
        public ulong? DataCanBeUsedInPresentations { get; set; }
        public ulong? Cc1Exempted { get; set; }
        public ulong? EnterpriseDealMinimumFee { get; set; }
        public int? EnterpriseDealMinimumFeeAmount { get; set; }
        public long? ConsumerRevenue { get; set; }
        public int? ConsumerRevenueMarginPercentage { get; set; }
        public int? ConsumerRevenuePercentageOn { get; set; }
        public int? ConsumerRevenuePercentageThroughCc { get; set; }
        public int? ConsumerRevenueTakeRate { get; set; }
        public double? ConsumerRevenueUplift { get; set; }
        public long? Opex { get; set; }
        public int? OpexPercentageOn { get; set; }
        public int? OpexPercentageThroughCc { get; set; }
        public int? OpexTakeRate { get; set; }
        public double? OpexUplift { get; set; }
        public long? WriteOffs { get; set; }
        public int? WriteOffsPercentageOn { get; set; }
        public int? WriteOffsPercentageThroughCc { get; set; }
        public int? WriteOffsTakeRate { get; set; }
        public double? WriteOffsUplift { get; set; }
        public DateTime? AccentureFirstCalMeetingDate { get; set; }
        public DateTime? AccentureFirstJointMeetingDate { get; set; }
        public string FactSetId { get; set; }
        public int? PartnerPhase { get; set; }
        public string IndexKey { get; set; }
        public ulong? IsAiroTarget { get; set; }
        public int? Classification { get; set; }
        public int? Phase { get; set; }
    }
}
