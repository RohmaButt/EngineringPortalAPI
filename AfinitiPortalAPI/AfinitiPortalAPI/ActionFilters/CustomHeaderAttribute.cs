using Microsoft.AspNetCore.Mvc.Filters;

namespace AfinitiPortalAPI.ActionFilters
{
    public class CustomHeaderAttribute : ResultFilterAttribute
    {
        //Response headers will be filled with refreshed/valid Token-key from API
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var _webApiContext = (WebApiContext)context.HttpContext.RequestServices.GetService(typeof(WebApiContext));
            context?.HttpContext?.Response.Headers.Add("Access-Control-Expose-Headers", "*");
            context?.HttpContext?.Response.Headers.Add("portal-api-token-key", _webApiContext.CrowdSSOToken);
            base.OnResultExecuting(context);
        }
    }
}
