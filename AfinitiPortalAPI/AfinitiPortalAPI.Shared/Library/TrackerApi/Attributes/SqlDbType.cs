using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Library.TrackerApi.Attributes
{
    public class SqlDbTypeAttribute : Attribute
    {
        public SqlDbTypeAttribute(SqlDbType dbType, int dataLength, bool nullable = true)
        {
            this.DBType = dbType;
            this.DataLength = dataLength;
            this.Nullable = nullable;
        }

        public SqlDbTypeAttribute(SqlDbType dbType, bool nullable = true)
        {
            this.DBType = dbType;
            this.Nullable = nullable;
            this.DataLength = (int?)null;
        }


        public SqlDbType DBType { get; set; }

        public int? DataLength { get; set; }

        public bool Nullable { get; set; }
    }
}
