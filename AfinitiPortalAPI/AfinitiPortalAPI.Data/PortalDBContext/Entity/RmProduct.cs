using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmProduct
    {
        public RmProduct()
        {
            RmDefinitionAssignments = new HashSet<RmDefinitionAssignment>();
            RmResourceModelDefinitions = new HashSet<RmResourceModelDefinition>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RmDefinitionAssignment> RmDefinitionAssignments { get; set; }
        public virtual ICollection<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
    }
}
