using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Authorization
{
    public class MenuPermissionModel
    {
        public string Title { get; set; }

        public int PermissionKey { get; set; }

        public int? ParentKey { get; set; }

        public string LinkTo { get; set; }

        public bool HasBullet { get; set; }

        public string Icon { get; set; }

        public string FontIcon { get; set; }

        public bool IsNewTab { get; set; }

        public bool IsLandingPage { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }
    }
}
