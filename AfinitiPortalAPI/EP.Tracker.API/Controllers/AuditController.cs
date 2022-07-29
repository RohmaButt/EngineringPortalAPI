using AfinitiPortalAPI.Shared.Library.TrackerApi.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Tracker.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class AuditController : ControllerBase
    {
        public AuditController()
        { }

        [HttpPost]
        public IActionResult CreateAuditLog([FromBody] AuditModel auditLog, [FromQuery] bool fromApi = false)
        {
            // Append Http related Info...
            auditLog.CreatedAt = DateTime.UtcNow;

            if (!fromApi) // from UI
            {
                auditLog.ClientIp = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "N/A";
                auditLog.ClientBrowser = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "N/A";
            }

            // Write Log...
            Log.Information("{@Id}{@Email}{@Path}{@ApiUrl}{@DomSelector}{@Properties}{@ActionCode}{@ActionType}{@CreatedAt}{@ClientIp}{@ClientBrowser}", 1, auditLog.Email, auditLog.Path, auditLog.ApiUrl, auditLog.DomSelector, auditLog.Properties, auditLog.ActionCode, (int)auditLog.ActionType, auditLog.CreatedAt, auditLog.ClientIp, auditLog.ClientBrowser);

            return Ok();
        }
    }
}
