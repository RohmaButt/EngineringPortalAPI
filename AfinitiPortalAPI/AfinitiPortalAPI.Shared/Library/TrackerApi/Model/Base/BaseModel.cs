using AfinitiPortalAPI.Shared.Library.TrackerApi.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Library.TrackerApi.Model
{
    public class BaseModel
    {
        [SqlDbType(SqlDbType.BigInt, false)]
        public long Id { get; set; }

        [SqlDbType(SqlDbType.DateTime2, false)]
        public DateTime CreatedAt { get; set; }
    }
}
