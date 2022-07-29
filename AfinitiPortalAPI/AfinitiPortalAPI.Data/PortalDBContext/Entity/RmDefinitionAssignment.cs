using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class RmDefinitionAssignment
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int DefinitionId { get; set; }
        public int? RevenueBracketId { get; set; }
        public int? ProductId { get; set; }
        public float? SharedAssignment { get; set; }
        public float? DedicatedAssignment { get; set; }

        public virtual RmResourceModelDefinition Definition { get; set; }
        public virtual RmProduct Product { get; set; }
        public virtual RmRevenueBracket RevenueBracket { get; set; }
    }
}
