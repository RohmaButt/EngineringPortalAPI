using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.ActionFilters
{
    public class CrowdAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Log.Information("OnAuthorizationAsync - Start");
            StringValues tokenHeader = StringValues.Empty;
            var isTokenFound = context?.HttpContext?.Request.Headers.TryGetValue(Constants.Api.CrowdTokenHeaderKey, out tokenHeader) ?? false;
            Log.Information("OnAuthorizationAsync - isTokenFound: ", isTokenFound);
            var token = tokenHeader.FirstOrDefault();
            Log.Information("OnAuthorizationAsync - token: ", token);
            if (!isTokenFound)
            {
                Log.Error("OnAuthorizationAsync - isTokenFound: ", isTokenFound);
                context.Result = new UnauthorizedResult();
            }

            //user name will be recing in all calls from frontend in Request Headers like token
            StringValues UserNameHeader = StringValues.Empty;
            var isUserNameHeader = context?.HttpContext?.Request.Headers.TryGetValue(Constants.Api.UserNameHeaderKey, out UserNameHeader) ?? false;
            if (!isUserNameHeader)
            {
                Log.Error("OnAuthorizationAsync - isUserNameHeader: ", isUserNameHeader);
                context.Result = new UnauthorizedResult();
            }
            var userName = UserNameHeader.FirstOrDefault();

            var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            var authorizationResult = await userService.ValidateCrowdToken(token, userName);
            Log.Information("OnAuthorizationAsync - authorizationResult.IsValid: ", authorizationResult?.IsValid);
            Log.Information("OnAuthorizationAsync - Completed");

            // Unauthorized...
            if (!(authorizationResult?.IsValid ?? false))
            {
                Log.Error("OnAuthorizationAsync - Completed with 401:authorizationResult.IsValid: ", authorizationResult.IsValid);
                context.Result = new UnauthorizedResult();
            }

            var email = authorizationResult?.Email?.ToLowerInvariant();

            // Fill WebApiContext...
            if (!string.IsNullOrWhiteSpace(email))
            {
                var serviceCatalogueRolesOfAuthenticatedUser = await userService.GetServiceCatalogueRolesWithCache(email);

                var _webApiContext = (WebApiContext)context.HttpContext.RequestServices.GetService(typeof(WebApiContext));
                _webApiContext.ServiceCatalogueRoles = serviceCatalogueRolesOfAuthenticatedUser;
                _webApiContext.UserName = authorizationResult?.UserName.ToLower();
                _webApiContext.Email = email;
                _webApiContext.CrowdSSOToken = authorizationResult.CrowdSSOToken;
            }
            else
                Log.Warning("Unable to fill WebApiContext on authorization. It may cause errors. Please check authentication mechanism.");
        }
    }
}
