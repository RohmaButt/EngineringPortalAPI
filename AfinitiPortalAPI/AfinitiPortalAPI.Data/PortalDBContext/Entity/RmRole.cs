using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmRole
    {
        public RmRole()
        {
            RmEmployeeRoleMappings = new HashSet<RmEmployeeRoleMapping>();
            RmPlannings = new HashSet<RmPlanning>();
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int DepartmentId { get; set; }
        public int? SubdepartmentId { get; set; }
        public int? TeamId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ShiftCount { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual RmSubdepartment Subdepartment { get; set; }
        public virtual RmTeam Team { get; set; }
        public virtual ICollection<RmEmployeeRoleMapping> RmEmployeeRoleMappings { get; set; }
        public virtual ICollection<RmPlanning> RmPlannings { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
    }
}
