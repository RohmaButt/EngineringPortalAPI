using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class ResourceModelRolesLookup
    {
        public int Id { get; set; }
        public string RoleResourceModel { get; set; }
        public int RoleGroupId { get; set; }
        public string PaycomSubDepartment { get; set; }
        public int? Status { get; set; }
        public int? IsDedicated { get; set; }
        public int? Shifts { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
