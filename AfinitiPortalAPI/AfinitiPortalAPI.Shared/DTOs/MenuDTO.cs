using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class MenuPermission
    {
        public string SortOrder { get; set; }
        public string DisplayText { get; set; }
        public Guid PermissionKey { get; set; }
        public Guid TypeKey { get; set; }
        public Guid? ParentKey { get; set; }
        public string URL { get; set; }
        public List<SubPermission> SubPermissions { get; set; }
        public string CSSClass { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class SubPermission
    {
        public string SortOrder { get; set; }
        public string DisplayText { get; set; }
        public Guid PermissionKey { get; set; }
        public Guid TypeKey { get; set; }
        public Guid? ParentKey { get; set; }
        public string URL { get; set; }
        public string CSSClass { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
