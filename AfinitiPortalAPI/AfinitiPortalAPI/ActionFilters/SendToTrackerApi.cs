using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared;
using AfinitiPortalAPI.Shared.ApiClient;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.ActionFilters
{
    public class SendToTrackerApi : ActionFilterAttribute
    {
        protected readonly TrackerApiClient _apiClient;
        protected readonly WebApiContext _webApiContext;

        public SendToTrackerApi(TrackerApiClient apiClient, WebApiContext webApiContext)
        {
            this._apiClient = apiClient;
            this._webApiContext = webApiContext;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await _apiClient.CreateAuditLog(
                        _webApiContext.Email,
                        null, // Path
                        context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : null, // ApiUrl
                        null, //  DomSelector
                        context?.HttpContext?.Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "N/A", // Ip
                        context.HttpContext.Request.Headers.ContainsKey("User-Agent") ? context.HttpContext.Request.Headers["User-Agent"].ToString() : "N/A", // Browser
                        null, // ActionCode
                        null // Properties
                        );
            }
            catch (Exception ex)
            {
                Log.Error("Unable to send log to Tracker API. Ex: {@ex}", ex);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
