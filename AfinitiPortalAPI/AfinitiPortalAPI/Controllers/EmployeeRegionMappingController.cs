using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class EmployeeRegionMappingController : BaseController
    {
        private readonly IEmployeeRegionMappingService _employeeRegionMappingService;

        public EmployeeRegionMappingController(IEmployeeRegionMappingService employeeRegionMappingService) // IOC design principle => (Dependency Injection)DI using Constructor injection
        {
            _employeeRegionMappingService = employeeRegionMappingService;
        }
        [HttpGet]
        [Route("GetAllEmployeeRegionMapping")]
        public async Task<IActionResult> GetEmployeeRegionMapping()
        {
            Log.Information("GetAllEmployeeRegionMapping:Started");
            dynamic data = null;
            try
            {
                data = await _employeeRegionMappingService.GetEmployeeRegionMapping();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllEmployeeRegionMapping: Unable to load data: ", ex);
                throw;
            }
            Log.Information("GetAllEmployeeRegionMapping:Completed");
            return Ok(data);
        }


        [HttpPost]
        [Route("AddOrUpdateEmployeeRegionMapping")]
        public async Task<ActionResult<RmEmployeeRegionalMappingDTO>> AddOrUpdateEmployeetRegionMapping([FromBody] RmEmployeeRegionalMappingDTO accountRegionMapping)
        {
            Log.Information("AddOrUpdateEmployeeRegionMapping:Started");
            try
            {
                RmEmployeeRegionalMappingDTO res = await _employeeRegionMappingService.AddOrUpdateEmployeeRegionMapping(accountRegionMapping);
                Log.Information("AddOrUpdateEmployeeRegionMapping:Completed");
                return Ok(res);
            }
            catch (Exception ex)
            {
                Log.Error("AddOrUpdateEmployeeRegionMapping: Unable to save data: ", ex);
                throw;
            }
        }
    }
}
