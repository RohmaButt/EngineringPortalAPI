using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class HermesQueueSwitchPlatform
    {
        public int Id { get; set; }
        public int? SwitchPlatformId { get; set; }
        public int? QueueId { get; set; }
        public int? DistributionPercentage { get; set; }
        public int? OwnerId { get; set; }
    }
}
