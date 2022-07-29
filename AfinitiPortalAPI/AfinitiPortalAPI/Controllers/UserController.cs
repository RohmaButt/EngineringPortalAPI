using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared;
using AfinitiPortalAPI.Shared.DTOs;
using AfinitiPortalAPI.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    [AllowAnonymous]
    [EnableCors("Policy1")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PortalDBContext _dbContext;

        public UserController(IUserService userService, PortalDBContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("GetImpersonatedUserInfo")]
        public async Task<IActionResult> GetImpersonatedUserInfo([FromBody] CredentialsDTO credentials)
        {
            AuthResult authResult = new();
            if (string.IsNullOrEmpty(credentials.UserName))
            {
                Log.Error("Incorrect user name. Please try again.");
                return Ok(authResult);
            }
            try
            {
                Log.Information("UserController:GetImpersonatedUserInfo Started");
                authResult = await _userService.GetImpersonatedUserInfo(credentials.UserName.ToLower());
                var defaultErrorMessage = "Incorrect user name, or you are not connected to the VPN. Please try again.";
                if (authResult == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, defaultErrorMessage);
                }
                if (string.IsNullOrEmpty(authResult.CrowdToken))
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, authResult.ErrorMessage ?? defaultErrorMessage);
                }
                Log.Information("UserController:GetImpersonatedUserInfo Completed");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return Ok(authResult);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDTO credentials)
        {
            AuthResult authResult = new();
            if (credentials is null || string.IsNullOrEmpty(credentials.UserName) || string.IsNullOrEmpty(credentials.Password))
            {
                Log.Error("Incorrect user name or password. Please try again.");
                authResult.ErrorMessage = "Missing user name or password. Please try again.";
                return Ok(authResult);
            }
            try
            {
                Log.Information("UserController:Login Started");
                authResult = await _userService.LogIn(credentials.UserName, credentials.Password);
                var defaultErrorMessage = "Incorrect user name or password, or you are not connected to the VPN. Please try again.";
                if (authResult == null)
                {
                    Log.Information("UserController:Login Error Completed: authResult == null");
                    return Unauthorized(defaultErrorMessage);
                }
                if (string.IsNullOrEmpty(authResult.CrowdToken))
                {
                    Log.Information("UserController:Login Error Completed: " + authResult.ErrorMessage);
                    return Unauthorized(authResult.ErrorMessage ?? defaultErrorMessage);
                }
            }
            catch (Exception e)
            {
                Log.Error("UserController:Login Exception" + e.InnerException);
                Log.Error("UserController:Login Exception" + e.Message);
                Log.Error(e.Message);
            }
            Log.Information("UserController:Login Completed");
            return Ok(authResult);
        }

        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut(string CrowdSSOToken)
        {
            //TODO remove user cookie
            //if (string.IsNullOrEmpty(crowdToken))
            //    Log.Error("Invalid crowdToken. Please try again.");
            bool authResult = false;
            try
            {
                Log.Information("UserController:LogOut Started");
                authResult = await _userService.LogOut(CrowdSSOToken);
                var defaultErrorMessage = "Incorrect user name or password, or you are not connected to the VPN. Please try again.";
                if (!authResult)
                    return StatusCode((int)HttpStatusCode.InternalServerError, defaultErrorMessage);
                Log.Information("UserController:LogOut Completed");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return Ok(authResult);
        }

        [HttpGet]
        [Route("ValidateCrowdSSOToken")]
        public async Task<IActionResult> ValidateCrowdSSOToken(string CrowdSSOToken, string UserName)
        {
            CrowdResult authResult = new();
            if (CrowdSSOToken is null || string.IsNullOrEmpty(CrowdSSOToken))
            {
                Log.Error("Incorrect user. Please try again.");
                return Ok(authResult);
            }
            try
            {
                Log.Information("UserController:Login Started");
                authResult = await _userService.ValidateCrowdToken(CrowdSSOToken, UserName);
                var defaultErrorMessage = "Incorrect user, or you are not connected to the VPN. Please try again.";
                if (authResult == null || string.IsNullOrEmpty(authResult.CrowdSSOToken) || CrowdSSOToken != authResult.CrowdSSOToken)
                    return StatusCode((int)HttpStatusCode.Unauthorized, defaultErrorMessage);
                Log.Information("UserController:Login Completed");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return Ok(authResult);
        }

        [HttpGet("getUserProfileData")]
        public async Task<IActionResult> GetUserProfileData([FromQuery] string email)
        {
            var response = await _userService.GenerateUserProfileData(email);
            return Ok(response);
        }
    }
}
