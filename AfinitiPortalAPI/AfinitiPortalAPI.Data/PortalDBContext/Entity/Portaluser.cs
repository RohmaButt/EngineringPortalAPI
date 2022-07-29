using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class Portaluser
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public string ApprovalStatus { get; set; }
        public int? StrikeCount { get; set; }
        public DateTime? LastSrikeDate { get; set; }
    }
}
