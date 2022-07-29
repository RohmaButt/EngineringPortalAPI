using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmResourceModelDefinition
    {
        public RmResourceModelDefinition()
        {
            RmDefinitionAssignments = new HashSet<RmDefinitionAssignment>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int PhaseId { get; set; }
        public int DepartmentId { get; set; }
        public int? SubdepartmentId { get; set; }
        public int? TeamId { get; set; }
        public int RoleId { get; set; }
        public int RevenueBracketId { get; set; }
        public int ProductId { get; set; }
        public int VersionNo { get; set; }
        public DateTime VersionDate { get; set; }
        public bool IsLatestVersion { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual RmPhase Phase { get; set; }
        public virtual RmProduct Product { get; set; }
        public virtual RmRevenueBracket RevenueBracket { get; set; }
        public virtual RmRole Role { get; set; }
        public virtual RmSubdepartment Subdepartment { get; set; }
        public virtual RmTeam Team { get; set; }
        public virtual ICollection<RmDefinitionAssignment> RmDefinitionAssignments { get; set; }
    }
}
