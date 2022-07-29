using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmRegion
    {
        public RmRegion()
        {
            RmAccountRegionalMappings = new HashSet<RmAccountRegionalMapping>();
            RmEmployeeRegionalMappings = new HashSet<RmEmployeeRegionalMapping>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubdepartmentId { get; set; }
        public int? TeamId { get; set; }
        public string Name { get; set; }
        public string RegionalManager { get; set; }
        public bool IsRegion { get; set; }
        public bool IsActive { get; set; }
        public bool IsImmutable { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual RmSubdepartment Subdepartment { get; set; }
        public virtual RmTeam Team { get; set; }
        public virtual ICollection<RmAccountRegionalMapping> RmAccountRegionalMappings { get; set; }
        public virtual ICollection<RmEmployeeRegionalMapping> RmEmployeeRegionalMappings { get; set; }
    }
}
