using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmEmployeeSwitchPlatformKnowledge
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int SwitchPlatformId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }
    }
}
