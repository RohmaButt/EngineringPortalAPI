using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmPhase
    {
        public RmPhase()
        {
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int DepartmentId { get; set; }
        public int? HermesPhaseId { get; set; }
        public string Name { get; set; }

        public virtual RmDepartment Department { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
    }
}
