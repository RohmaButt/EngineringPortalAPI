using AfinitiPortalAPI.Shared.Library.TrackerApi.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Library.TrackerApi.Model
{
    public class AuditModel : BaseModel
    {
        [SqlDbType(SqlDbType.NVarChar, 255, true)]
        public string Email { get; set; }

        [SqlDbType(SqlDbType.NVarChar, 1000, true)]
        public string Path { get; set; }

        [SqlDbType(SqlDbType.NVarChar, 1000, true)]
        public string ApiUrl { get; set; }

        [SqlDbType(SqlDbType.NVarChar, 1000, true)]
        public string DomSelector { get; set; }

        [SqlDbType(SqlDbType.NVarChar, 50, false)]
        public string ClientIp { get; set; } = "N/A";

        [SqlDbType(SqlDbType.NVarChar, 1000, true)]
        public string ClientBrowser { get; set; }

        [SqlDbType(SqlDbType.NVarChar, true)]
        public string Properties { get; set; }

        [SqlDbType(SqlDbType.NVarChar, 50, true)]
        public string ActionCode { get; set; }

        [SqlDbType(SqlDbType.TinyInt, false)]
        public ActionType ActionType { get; set; }
    }
}
