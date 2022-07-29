using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class EmployeeRoleMappingController : BaseController
    {
        private readonly IEmployeeRoleMappingService _employeeRoleMappingService;

        public EmployeeRoleMappingController(IEmployeeRoleMappingService employeeRoleMappingService) // IOC design principle => (Dependency Injection)DI using Constructor injection
        {
            _employeeRoleMappingService = employeeRoleMappingService;
        }
        [HttpGet]
        [Route("GetAllEmployeeRoleMapping")]
        public async Task<IActionResult> GetAllEmployeeRoleMapping()
        {
            Log.Information("GetAllEmployeeRoleMapping:Started");
            dynamic data;
            try
            {
                data = await _employeeRoleMappingService.GetEmployeeRoleMapping();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllEmployeeRoleMapping: Unable to load data: ", ex);
                throw;
            }
            Log.Information("GetAllEmployeeRoleMapping:Completed");
            return Ok(data);
        }


        [HttpPost]
        [Route("AddOrUpdateEmployeeRoleMapping")]
        public async Task<ActionResult<RmEmployeeRoleMappingDTO>> AddOrUpdateEmployeeRoleMapping([FromBody] RmEmployeeRoleMappingDTO accountRegionMapping)
        {
            Log.Information("AddOrUpdateEmployeeRoleMapping:Started");
            try
            {
                RmEmployeeRoleMappingDTO res = await _employeeRoleMappingService.AddOrUpdateEmployeeRoleMapping(accountRegionMapping);
                Log.Information("AddOrUpdateEmployeeRoleMapping:Completed");
                return Ok(res);
            }
            catch (Exception ex)
            {
                Log.Error("AddOrUpdateEmployeeRoleMapping: Unable to save data: ", ex);
                throw;
            }
        }
    }
}
