using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmEmployeeRoleMapping
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int RoleId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }

        public virtual RmRole Role { get; set; }
    }
}
