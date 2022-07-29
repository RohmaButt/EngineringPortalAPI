using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AfinitiPortalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        // GET: api/<EchoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Yolo:)" };
        }

        [HttpGet("version")]
        public string GetVersion()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        }
    }
}
