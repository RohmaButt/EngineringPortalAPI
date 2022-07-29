using AfinitiPortalAPI.Shared.Authorization;
using AfinitiPortalAPI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Extensions
{
    public static class CustomExtensions
    {
        public static void SetLandingPage(this List<PermissionModel> input, string linkTo)
        {
            input.Add(new PermissionModel(
               PermissionType.LandingPage,
               Constants.Authorization.LANDING_PAGE_COMPONENT_CODE,
               new KeyValuePair<string, object>(
                   "LANDING_PAGE",
                   linkTo)));
        }

        public static void SetLandingPage(this List<MenuPermissionModel> input, string linkTo)
        {
            input.Where(x => x.LinkTo == linkTo).Single().IsLandingPage = true;
        }
    }
}
