using System;
using System.Collections.Generic;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext.Entity
{
    public partial class TsPeriod
    {
        public int Id { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public bool? LockStatus { get; set; }
        public DateTime InsertionDate { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
    }
}
