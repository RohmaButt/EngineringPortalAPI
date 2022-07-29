using AfinitiPortalAPI.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AfinitiPortalAPI.Controllers
{
    [ServiceFilter(typeof(SendToTrackerApi))]
    [CrowdAuthorization]
    [EnableCors("Policy1")]
    [ApiController]
    [Route("[controller]")]
    [CustomHeader]
    public class BaseController : ControllerBase
    { }
}