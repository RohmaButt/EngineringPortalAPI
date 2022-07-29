using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class OrganizationChartController : BaseController
    {
        private readonly IOrganizationChartService _organizationChartService;
        public OrganizationChartController(IOrganizationChartService organizationChartService)
        {
            _organizationChartService = organizationChartService;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            Log.Information("OrganizationChartController: GetAllUsers Started");
            GetResponse<List<AfinitiUser>> response = new();
            try
            {
                response = await _organizationChartService.GetAllUsers();
            }
            catch (Exception ex)
            {
                Log.Error("OrganizationChartController: GetAllUsers", ex.Message);
            }
            Log.Information("OrganizationChartController: GetAllUsers Completed");
            return Ok(response);
        }

        [HttpGet]
        [Route("GetOrgDataFlat")]
        public async Task<IActionResult> GetUserOrgDataFlat([FromQuery] OrgChartRequestObj model)
        {
            GetResponse<List<PaycomEmployeeFull_DTOFlat>> OrgDataResult = new();
            try
            {
                if (string.IsNullOrEmpty(model.WorkEmail))
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Issue in input params");
                Log.Information("OrganizationChartController:GetUserOrgData Started");
                OrgDataResult = await _organizationChartService.GetUserOrgDataFlat(model);
                var defaultErrorMessage = "Incorrect user, or you are not connected to the VPN. Please try again.";
                if (OrgDataResult == null)
                    return StatusCode((int)HttpStatusCode.Unauthorized, defaultErrorMessage);
            }
            catch (Exception e)
            {
                Log.Error("OrganizationChartController:GetUserOrgData - ", e.Message);
            }
            Log.Information("OrganizationChartController:GetUserOrgData Completed");
            return Ok(OrgDataResult);
        }

        /*      [HttpGet]
              [Route("GetOrgData")]
              public async Task<IActionResult> GetUserOrgData([FromQuery] OrgChartRequestObj model)
              {
                  //if (string.IsNullOrEmpty(model.WorkEmail))
                  //    return StatusCode((int)HttpStatusCode.BadRequest, "People manager name is missing.");
                  //if (string.IsNullOrEmpty(model.FetchTillLastEdge.ToString()))
                  //    return StatusCode((int)HttpStatusCode.BadRequest, "Reportees level is missing.");
                  //if (string.IsNullOrEmpty(model.WorkStatus))
                  //    return StatusCode((int)HttpStatusCode.BadRequest, "Work Status is missing.");

                  List<PaycomEmployeeFull_DTO> OrgDataResult = new();
                  try
                  {
                      Log.Information("OrganizationChartController:GetUserOrgData Started");
                      OrgDataResult = await _organizationChartService.GetUserOrgData(model);
                      var defaultErrorMessage = "Incorrect user, or you are not connected to the VPN. Please try again.";
                      if (OrgDataResult == null)
                          return StatusCode((int)HttpStatusCode.Unauthorized, defaultErrorMessage);
                  }
                  catch (Exception e)
                  {
                      Log.Error("OrganizationChartController:GetUserOrgData - ", e.Message);
                  }
                  Log.Information("OrganizationChartController:GetUserOrgData Completed");
                  return Ok(OrgDataResult);
              }*/
    }
}
