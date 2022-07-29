using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class AccountRegionMappingController : BaseController
    {
        private readonly IAcctRegionMappingService _acctRegionMappingService;

        public AccountRegionMappingController(IAcctRegionMappingService acctRegionMappingService) // IOC design principle => (Dependency Injection)DI using Constructor injection
        {
            _acctRegionMappingService = acctRegionMappingService;
        }
        [HttpGet]
        [Route("GetAllAccountRegionMapping")]
        public async Task<IActionResult> GetAccountRegionMapping()
        {
            Log.Information("GetAccountRegionMapping:Started");
            dynamic data = null;
            try
            {
                data = await _acctRegionMappingService.GetAccountRegionMapping();
            }
            catch (Exception ex)
            {
                Log.Error("GetAccountRegionMapping: Unable to load data: ", ex);
            }
            Log.Information("GetAccountRegionMapping:Completed");
            return Ok(data);
        }


        [HttpPost]
        [Route("AddOrUpdateAccountRegionMapping")]
        public async Task<ActionResult<RmAccountRegionalMappingDTO>> AddOrUpdateAccountRegionMapping([FromBody] RmAccountRegionalMappingDTO accountRegionMapping)
        {
            Log.Information("AddOrUpdateAccountRegionMapping:Started");
            try
            {
                await _acctRegionMappingService.AddOrUpdateAccountRegionMapping(accountRegionMapping);
                Log.Information("AddOrUpdateAccountRegionMapping:Completed");
                return CreatedAtAction("AddOrUpdateAccountRegionMapping", new { id = accountRegionMapping.Id }, accountRegionMapping);
            }
            catch (Exception ex)
            {
                Log.Error("AddOrUpdateAccountRegionMapping: Unable to save data: ", ex);
                throw;
            }
        }
    }
}
