using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class LookupsController : BaseController
    {
        private readonly ILookupsService _lookupsService;
        public LookupsController(ILookupsService lookupsService)
        {
            _lookupsService = lookupsService;
        }

        [HttpGet]
        [Route("GetPaycomDepartmentsInfo")]
        public async Task<IActionResult> GetPaycomDepartmentsInfo()
        {
            Log.Information("LookupsController: GetPaycomDepartmentsInfo Started");
            GetResponse<List<PaycomDepartments>> response = new();
            try
            {
                response = await _lookupsService.GetPaycomDepartmentsInfo();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetPaycomDepartmentsInfo", ex.Message);
            }
            Log.Information("LookupsController: GetPaycomDepartmentsInfo Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetGroups")]
        public async Task<IActionResult> GetGroups()
        {
            Log.Information("LookupsController: GetGroups Started");
            GetResponse<List<ResourceModelRolesGroupDTO>> response = new();
            try
            {
                response = await _lookupsService.GetGroups();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetGroups", ex.Message);
            }
            Log.Information("LookupsController: GetGroups Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSwitchPlatforms")]
        public async Task<IActionResult> GetSwitchPlatforms()
        {
            Log.Information("LookupsController: GetSwitchPlatforms Started");
            GetResponse<List<SwitchPlatformsDTO>> response = new();
            try
            {
                response = await _lookupsService.GetSwitchPlatforms();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetSwitchPlatforms", ex.Message);
            }
            Log.Information("LookupsController: GetSwitchPlatforms Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSwitchProviders")]
        public async Task<IActionResult> GetSwitchProviders()
        {
            Log.Information("LookupsController: GetSwitchProviders Started");
            GetResponse<List<SwitchProvidersDTO>> response = new();
            try
            {
                response = await _lookupsService.GetSwitchProviders();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetSwitchProviders", ex.Message);
            }
            Log.Information("LookupsController: GetSwitchProviders Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSIDEmployees")]
        public async Task<IActionResult> GetSIDEmployees()
        {
            Log.Information("LookupsController: GetSIDEmployees Started");
            GetResponse<List<NewEmployeeSwitchKnowledgeDTO>> response = new();
            try
            {
                response = await _lookupsService.GetSIDEmployees();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetSIDEmployees", ex.Message);
            }
            Log.Information("LookupsController: GetSIDEmployees Completed");
            return Ok(response);
        }


        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles(string teamName)
        {
            Log.Information("LookupsController: GetRoles Started");
            GetResponse<List<RmRoleDTOTrimmed>> response = new();
            try
            {
                response = await _lookupsService.GetRoles(teamName);
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetRoles", ex.Message);
            }
            Log.Information("LookupsController: GetRoles Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            Log.Information("LookupsController: GetDepartments Started");
            GetResponse<List<RmDepartmentDTO>> response = new();
            try
            {
                response = await _lookupsService.GetDepartments();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetDepartments", ex.Message);
            }
            Log.Information("LookupsController: GetDepartments Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSubDepartments")]
        public async Task<IActionResult> GetSubDepartments()
        {
            Log.Information("LookupsController: GetSubDepartments Started");
            GetResponse<List<RmSubdepartmentDTO>> response = new();
            try
            {
                response = await _lookupsService.GetSubDepartments();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetSubDepartments", ex.Message);
            }
            Log.Information("LookupsController: GetSubDepartments Completed");
            return Ok(response);
        }
        [HttpGet]
        [Route("GetTeams")]
        public async Task<IActionResult> GetTeams()
        {
            Log.Information("LookupsController: GetTeams Started");
            GetResponse<List<RmTeamDTO>> response = new();
            try
            {
                response = await _lookupsService.GetTeams();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetTeams", ex.Message);
            }
            Log.Information("LookupsController: GetTeams Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetJiraIssueCategories")]
        public async Task<IActionResult> GetJiraIssueCategories()
        {
            Log.Information("LookupsController: GetJiraIssues Started");
            GetResponse<List<TsJiraIssueCategoryDTO>> response = new();
            try
            {
                response = await _lookupsService.GetJiraIssueCategories();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetJiraIssueCategories", ex.Message);
            }
            Log.Information("LookupsController: GetJiraIssueCategories Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetJiraIssueTypes")]
        public async Task<IActionResult> GetJiraIssueTypes()
        {
            Log.Information("LookupsController: GetJiraIssueTypes Started");
            GetResponse<List<TsJiraIssueTypeDTO>> response = new();
            try
            {
                response = await _lookupsService.GetJiraIssueTypes();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetJiraIssueTypes", ex.Message);
            }
            Log.Information("LookupsController: GetJiraIssueTypes Completed");
            return Ok(response);
        }
        [HttpGet]
        [Route("GetHermesAccountsForJiraIssues")]
        public async Task<IActionResult> GetHermesAccountsForJiraIssues()
        {
            Log.Information("LookupsController: GetHermesAccountsForJiraIssues Started");
            GetResponse<List<TsJiraIssueTypeDTO>> response = new();
            try
            {
                response = await _lookupsService.GetHermesAccountsForJiraIssues();
            }
            catch (Exception ex)
            {
                Log.Error("LookupsController: GetHermesAccountsForJiraIssues", ex.Message);
            }
            Log.Information("LookupsController: GetHermesAccountsForJiraIssues Completed");
            return Ok(response);
        }
    }
}
