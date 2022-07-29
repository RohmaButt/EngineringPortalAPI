using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class ResourceModelRolesGroup
    {
        public int Id { get; set; }
        public string RoleGroupName { get; set; }
        public int RoleGroupId { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
