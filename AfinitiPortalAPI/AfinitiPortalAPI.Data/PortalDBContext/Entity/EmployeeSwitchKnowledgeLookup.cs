using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class EmployeeSwitchKnowledgeLookup
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public int SidregionId { get; set; }
        public int? SwitchProviderId { get; set; }
        public int? SwitchPlatformId { get; set; }
        public DateTime EtlDate { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
