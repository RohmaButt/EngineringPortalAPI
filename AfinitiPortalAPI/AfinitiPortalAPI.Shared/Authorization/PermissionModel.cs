using AfinitiPortalAPI.Shared.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AfinitiPortalAPI.Shared.Authorization
{
    public class PermissionModel
    {
        #region Ctor
        public PermissionModel()
        { }

        public PermissionModel(PermissionType permissionType, string componentCode, Dictionary<string, object> properties)
        {
            this.Type = permissionType;
            this.Code = componentCode;
            this.Props = properties;
        }

        public PermissionModel(PermissionType permissionType, string componentCode, params KeyValuePair<string, object>[] properties)
        {
            this.Type = permissionType;
            this.Code = componentCode;
            this.Props = properties.ToDictionary(k => k.Key, v => v.Value);
        }

        public PermissionModel(PermissionType permissionType, string componentCode)
        {
            this.Type = permissionType;
            this.Code = componentCode;
        }
        #endregion

        public PermissionType Type { get; set; }

        public string Code { get; set; }

        public Dictionary<string, object> Props { get; set; }

        public void AddProperty(string key, object value)
        {
            this.Props.Add(key, value);
        }
    }
}