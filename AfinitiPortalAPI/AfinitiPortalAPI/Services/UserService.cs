using AfinitiPortalAPI.Services.Base;
using AfinitiPortalAPI.Shared;
using AfinitiPortalAPI.Shared.Authorization;
using AfinitiPortalAPI.Shared.DTOs;
using AfinitiPortalAPI.Shared.Shared;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AfinitiPortalAPI.Shared.Enums;
using SubDept = AfinitiPortalAPI.Shared.Constants.Paycom.SubdepartmentCodes;
using Dept = AfinitiPortalAPI.Shared.Constants.Paycom.DepartmentCodes;
using Roles = AfinitiPortalAPI.Shared.Constants.Paycom.RoleNames;
using Levels = AfinitiPortalAPI.Shared.Constants.Paycom.Levels;
using Components = AfinitiPortalAPI.Shared.Constants.Authorization.Components;
using Countries = AfinitiPortalAPI.Shared.Constants.Paycom.Countries;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.Extensions;
using AfinitiPortalAPI.Shared.Authorization.CrowdSSO;

namespace AfinitiPortalAPI.services
{
    public interface IUserService
    {
        public Task<CrowdResult> ValidateCrowdToken(string crowdToken, string UserName);
        public Task<bool> LogOut(string CrowdSSOToken);
        public Task<AuthResult> LogIn(string userName, string password);
        public Task<AuthResult> GetImpersonatedUserInfo(string UserName);
        Task<List<ServiceCatalogueRoleDto>> GetServiceCatalogueRolesWithCache(string email);
        Task<GetResponse<List<PermissionModel>>> GenerateUserProfileData(string email);
    }
    public class UserService : BaseService, IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly PortalDBContext _dbContext;
        private readonly HttpContext _httpContext;
        private readonly string _crowdCookieName = "crowd.CookieName";
        private readonly string _jSessionIdCookieName = "crowd.jSessionIdCookieName";

        public UserService(IOptions<AppSettings> appSettings, HttpClient httpClient, PortalDBContext dbContext, IHttpContextAccessor httpContextAccessor, IMemoryCache cacheProvider, WebApiContext webApiContext)
        : base(dbContext, cacheProvider, webApiContext, appSettings)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        private async Task<CrowdResult> GetCrowdTokenFromUserName(string UserName)
        {
            Log.Information("UserService: GetCrowdTokenFromUserName Started");
            if (string.IsNullOrWhiteSpace(UserName))
            {
                Log.Error("UserService: GetCrowdTokenFromUserName  - Error with UserName null");
                return null;
            }
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string url = $"{_appSettings.CSTS_API_URL}/rest/CreateCrowdTokenByUserName?userName={UserName}";
                    var responseMessage = client.GetAsync(url).Result;
                    var isSuccess = responseMessage.IsSuccessStatusCode;
                    if (isSuccess)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync();
                        var responseJson = JObject.Parse(response.Result);
                        Log.Information("UserService: GetCrowdTokenFromUserName  - Completed");
                        return new CrowdResult
                        {
                            // Email = responseJson["Email"]?.Value<string>(),
                            UserName = responseJson["UserName"]?.Value<string>(),
                            CrowdSSOToken = responseJson["CrowdSSOToken"]?.Value<string>(),
                            IsValid = true
                        };
                    }
                    else
                    {
                        throw new Exception(responseMessage.StatusCode.ToString());
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "UserService: GetCrowdTokenFromUserName - Error caught while validating Crowd token against CSTS API");
                    return null;
                }
            }
        }

        public async Task<AuthResult> GetImpersonatedUserInfo(string UserName)
        {
            //1. Create CrowdToken using username
            //2. If TOken gets user info then Send Username to get UserProfile from SecurityLayer
            Log.Information("UserService: GetImpersonatedUserInfo - Started");
            Root rootObj = null;
            string email = string.Empty;
            try
            {
                CrowdResult crowdResult = await GetCrowdTokenFromUserName(UserName);
                if (!string.IsNullOrEmpty(crowdResult.CrowdSSOToken) && !string.IsNullOrEmpty(crowdResult.UserName))
                {
                    using (var client = new HttpClient())
                    {
                        Log.Information("UserService: GetImpersonatedUserInfo - In progress");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var userContents = JsonConvert.SerializeObject(new LoginRequestModel
                        {
                            CrowdSSOToken = crowdResult.CrowdSSOToken
                        });
                        string currentUrl = $"{_appSettings.CSTS_API_URL}/afinitiportal/GetUsersSecurityDataByToken_AfinitiPortal";
                        var stringContent = new StringContent(userContents, Encoding.UTF8, "application/json");
                        var responseMessage = client.PostAsync(currentUrl, stringContent).Result;
                        var isSuccess = responseMessage.IsSuccessStatusCode;
                        if (isSuccess)
                        {
                            var response = responseMessage.Content.ReadAsStringAsync();
                            rootObj = JsonConvert.DeserializeObject<Root>(response.Result);
                        }
                        else
                        {
                            Log.Error("UserService: GetImpersonatedUserInfo-exception:" + responseMessage.StatusCode.ToString());
                            throw new Exception(responseMessage.StatusCode.ToString());
                        }
                    }
                    Log.Information("UserService: GetImpersonatedUserInfo - InProgress");
                    var PaycomUserInfo = _dbContext.PaycomEmployeeFulls.FirstOrDefault(f => f.ChangeStatus == "CURRENT STATUS" && f.WorkEmail == string.Concat(UserName, "@afiniti.com"));
                    var obj = new AuthResult
                    {
                        Email = rootObj?.Response?.Email != null ? rootObj?.Response?.Email : PaycomUserInfo?.WorkEmail,
                        CrowdToken = rootObj?.Response?.CrowdObj?.CrowdSSOToken,
                        JSessionId = rootObj?.Response?.CrowdObj?.JSessionID,
                        UserName = rootObj?.Response?.UserName,
                        UserProfileData = this.GenerateUserProfileData(rootObj?.Response?.Email != null ? rootObj?.Response?.Email : PaycomUserInfo?.WorkEmail).GetAwaiter().GetResult()?.Data,
                        PaycomUserObjInfo = new PaycomUserObjInfo()
                        {
                            Department = PaycomUserInfo?.Department,
                            Work_Email = PaycomUserInfo?.WorkEmail,
                            Position = PaycomUserInfo?.PositionTitle,
                            Position_Code = PaycomUserInfo?.PositionCode,
                            Employee_Code = PaycomUserInfo?.EmployeeCode,
                            Role_Name = PaycomUserInfo?.RoleName,
                            Level = PaycomUserInfo?.Level,
                            People_Manager = PaycomUserInfo?.PeopleManager,
                            TwoLevelBelowCeoCode = PaycomUserInfo?.TwoLevelBelowCeoCode,
                            WorkStatus = PaycomUserInfo?.WorkStatus,
                            ChangeStatus = PaycomUserInfo?.ChangeStatus,
                            FirstSupervisor = PaycomUserInfo?.FirstSupervisor,
                            FirstSupervisorCode = PaycomUserInfo?.FirstSupervisorCode,
                            FirstSupervisorDepartment = PaycomUserInfo?.FirstSupervisorDepartment,
                            FirstSupervisorWorkEmail = PaycomUserInfo?.FirstSupervisorWorkEmail,
                            FirstSupervisorWorkStatus = PaycomUserInfo?.FirstSupervisorWorkStatus
                        }
                    };
                    Log.Information("UserService: GetImpersonatedUserInfo - Completed");
                    return obj;
                }
                else
                {
                    Log.Information("Error caught while validating Crowd token");
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error caught while validating Crowd token against CSTS API");
                return null;
            }
        }

        public async Task<AuthResult> LogIn(string UserName, string Password)
        {
            Log.Information("UserService: LogIn - Started");
            RootObject rootObj = null;
            string email = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    Log.Information("UserService: LogIn - In progress");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var userContents = JsonConvert.SerializeObject(new RootLoginRequestModel
                    {
                        model = new LoginRequestModel { UserName = UserName, Password = Password }
                    });
                    string currentUrl = $"{_appSettings.CSTS_API_URL}/afinitiportal/AuthenticateAndAuthorizeByCrowd_AfinitiPortal";
                    var stringContent = new StringContent(userContents, Encoding.UTF8, "application/json");
                    var responseMessage = client.PostAsync(currentUrl, stringContent).Result;
                    var isSuccess = responseMessage.IsSuccessStatusCode;
                    if (isSuccess)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync();
                        rootObj = JsonConvert.DeserializeObject<RootObject>(response.Result);
                    }
                    else
                    {
                        Log.Error("UserService: LogIn-exception:" + responseMessage.StatusCode.ToString());
                        throw new Exception(responseMessage.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error caught while validating Crowd token against CSTS API");
                return null;
            }
            Log.Information("UserService: LogIn - InProgress");
            var PortalUser = _dbContext.Portalusers.SingleOrDefault(x => x.UserName == UserName);
            if (PortalUser == null)
            {
                Log.Information("UserService: LogIn - PortalUser == null");
                email = (await ValidateCrowdToken(rootObj?.Response?.CrowdSSOToken, UserName))?.Email;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    PortalUser = new Portaluser
                    {
                        UserName = UserName.ToLower(),
                        Email = email.ToLower(),
                        ApprovalStatus = "1",
                        EmployeeCode = "000",
                        IsActive = true,
                        IsAdmin = false
                    };
                    await _dbContext.Portalusers.AddAsync(PortalUser);
                    await _dbContext.SaveChangesAsync();
                }
                Log.Information("UserService: LogIn - new PortalUser saved");
                //TODO: SignalR to send Email OR WebSocket to send Notification msg
            }
            else
            {     //Strike count logic for invalid credentials - START
                Log.Information("UserService: LogIn - PortalUser != null");
                if (string.IsNullOrEmpty(rootObj?.Response?.CrowdSSOToken) || string.IsNullOrEmpty(rootObj?.Response?.Email))
                {
                    int InvalidLoginStrike = (int)PortalUser?.StrikeCount; //current strike count of user in portal
                    string errorMessage = string.Empty;
                    switch (InvalidLoginStrike)
                    {
                        case 0:  //1st strike just hit 
                            PortalUser.StrikeCount = 1;
                            errorMessage = "Invalid credentials! You have 2 strikes left.";
                            break;
                        case 1:  //2nd strike just hit 
                            PortalUser.StrikeCount = 2;
                            errorMessage = "Invalid credentials! You have 1 strike left.";
                            break;
                        case 2: //3rd strike just hit 
                        case 3://3rd strike is already hit to AD so just update LastSrikeDate of user
                            PortalUser.StrikeCount = 3;
                            errorMessage = "Invalid credentials! Please contact service desk for resetting your password.";
                            break;
                        default:
                            break;
                    }
                    PortalUser.LastSrikeDate = DateTime.Now;
                    _dbContext.Portalusers.Update(PortalUser);
                    _dbContext.SaveChanges();
                    Log.Information("UserService: LogIn - PortalUser strikeCount saved:" + InvalidLoginStrike);
                    Log.Information("UserService: LogIn - PortalUser strikeCount saved:" + errorMessage);
                    return new AuthResult
                    {
                        ErrorMessage = errorMessage
                    };
                }
                else // if succesfully authenticated from Crowd then set StrikeCount=0 and LastSrikeDate will be last Login time of user
                {
                    Log.Information("UserService: LogIn - PortalUser strikeCount set to 0");
                    PortalUser.StrikeCount = 0;
                    PortalUser.LastSrikeDate = DateTime.Now;
                    _dbContext.Portalusers.Update(PortalUser);
                    _dbContext.SaveChanges();
                }
            }
            //Strike count logic for invalid credentials - END

            //TODO: (If)
            // Add Crowd token cookie
            //AddAuthCookies(rootObj?.Response?.CrowdObj?.CrowdSSOToken, rootObj?.Response?.CrowdObj?.JSessionID, _httpContext.Request, _httpContext.Response);
            Log.Information("UserService: LogIn - Almost completed");
            var PaycomUserInfo = _dbContext.PaycomEmployeeFulls.FirstOrDefault(f => f.ChangeStatus == "CURRENT STATUS" && f.WorkEmail == string.Concat(UserName, "@afiniti.com"));
            Log.Information("UserService: LogIn - Completed");
            return new AuthResult
            {
                Email = rootObj?.Response?.Email,
                CrowdToken = rootObj?.Response?.CrowdSSOToken,
                JSessionId = rootObj?.Response?.JSessionID,
                UserName = rootObj?.Response?.UserName,
                UserProfileData = this.GenerateUserProfileData(rootObj?.Response?.Email != null ? rootObj?.Response?.Email : PaycomUserInfo?.WorkEmail).GetAwaiter().GetResult()?.Data,
                PaycomUserObjInfo = new PaycomUserObjInfo()
                {
                    Department = PaycomUserInfo?.Department,
                    Work_Email = PaycomUserInfo?.WorkEmail,
                    Position = PaycomUserInfo?.PositionTitle,
                    Position_Code = PaycomUserInfo?.PositionCode,
                    Employee_Code = PaycomUserInfo?.EmployeeCode,
                    Role_Name = PaycomUserInfo?.RoleName,
                    Level = PaycomUserInfo?.Level,
                    People_Manager = PaycomUserInfo?.PeopleManager,
                    TwoLevelBelowCeoCode = PaycomUserInfo?.TwoLevelBelowCeoCode,
                    WorkStatus = PaycomUserInfo?.WorkStatus,
                    ChangeStatus = PaycomUserInfo?.ChangeStatus,
                    FirstSupervisor = PaycomUserInfo?.FirstSupervisor,
                    FirstSupervisorCode = PaycomUserInfo?.FirstSupervisorCode,
                    FirstSupervisorDepartment = PaycomUserInfo?.FirstSupervisorDepartment,
                    FirstSupervisorWorkEmail = PaycomUserInfo?.FirstSupervisorWorkEmail,
                    FirstSupervisorWorkStatus = PaycomUserInfo?.FirstSupervisorWorkStatus
                }
            };
        }

        private void AddAuthCookies(string crowdToken, string jSessionId, HttpRequest request, HttpResponse response)
        {
            var host = request.Host.Host;
            var domain = host;////"http://localhost:3000";//
                              //try
                              //{
                              //    if (_domainParser.IsValidDomain(host))
                              //    {
                              //        domain = $".{_domainParser.Get(host).RegistrableDomain}";
                              //    }
                              //}
                              //catch
                              //{
                              //    // Do nothing
                              //}
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Domain = domain
            };
            response.Cookies.Append(_crowdCookieName, crowdToken);
            response.Cookies.Append(_jSessionIdCookieName, jSessionId, cookieOptions);
        }

        public async Task<CrowdResult> ValidateCrowdToken(string crowdToken, string UserName)
        {
            Log.Information("UserService: ValidateCrowdToken Started");
            if (string.IsNullOrWhiteSpace(crowdToken))
            {
                Log.Error("UserService: ValidateCrowdToken  - Completed with crowd null value");
                return null;
            }
            try
            {
                Log.Information("UserService: ValidateCrowdToken - In progress");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(new { CrowdSSOToken = crowdToken, UserName = UserName });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_appSettings.CSTS_API_URL}/afinitiportal/AuthenticateUserByCrowd_AfinitiPortal", data);
                var result = await response.Content.ReadAsStringAsync();

                var responseJson = result.FromJson<CrowdAuthenticationResponse>();

                Log.Information("UserService: ValidateCrowdToken  - Completed");
                //if (responseJson.Response.AuthenticationCode == 1)
                //{
                return new CrowdResult
                {
                    Email = responseJson.Response.Email,
                    UserName = responseJson.Response.UserName,
                    CrowdSSOToken = responseJson.Response.CrowdSSOToken,
                    IsValid = responseJson.Response.AuthenticationCode == 1 ? true : false,
                    JSessionID = responseJson.Response.JSessionID,
                    ResponseMessage = responseJson.Response.AuthenticationMetaData
                };
                //}
                //else if (responseJson.Response.AuthenticationCode == 0)
                //{

                //}

            }
            catch (Exception e)
            {
                Log.Error(e, "UserService: VzalidateCrowdToken - Error caught while validating Crowd token against CSTS API");
                return null;
            }
        }

        public async Task<bool> LogOut(string CrowdSSOToken)
        {
            string crowdToken = CrowdSSOToken;// _httpContext.Request.Cookies[_crowdCookieName].ToString();
            if (string.IsNullOrWhiteSpace(crowdToken))
            {
                Log.Error("UserService: LogOut - Bypassing Crowd log out as Crowd token is not set in cookies");
                return true;
            }
            var success = false;
            try
            {
                var json = JsonConvert.SerializeObject(new { CrowdSSOToken = crowdToken });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_appSettings.CSTS_API_URL}custom/rest/Logout", data);
                var result = await response.Content.ReadAsStringAsync();
                success = JObject.Parse(result)["LoggedOut"].Value<bool>();
                if (!success)
                    Log.Error(result, "UserService: LogOut - Unsuccessful log out via CSTS - API responded with success flag set to false");
            }
            catch (Exception e)
            {
                Log.Error(e, "UserService: LogOut - Error caught while logging out in CSTS");
            }
            if (success)
            {
                _httpContext.Response.Cookies.Delete(_crowdCookieName);
                _httpContext.Response.Cookies.Delete(_jSessionIdCookieName);
            }
            Log.Information("UserService: LogOut Completed");
            return success;
        }

        public async Task<GetResponse<List<PermissionModel>>> GenerateUserProfileData(string email)
        {
            GetResponse<List<PermissionModel>> response = new GetResponse<List<PermissionModel>>();
            List<PermissionModel> result = new List<PermissionModel>();

            #region Collect required DB Data
            var employee = await this.GetPaycomEmployeeFullDataWithCache(email);

            var serviceCatalogueRoleData = await this.GetServiceCatalogueRolesWithCache(email);

            // Generate Display Catalogue Info according to user's role...
            var displayCatalogueInfo = serviceCatalogueRoleData.Select(x =>
            {
                if (x.RoleName == Roles.HeadOfEngineering)
                    return $"{x.RoleName} / {x.Portfolio}";
                else if (x.RoleName == Roles.ServiceGroupLead || x.RoleName == Roles.ServiceGroupArchitect || x.RoleName == Roles.ServiceGroupManager)
                    return $"{x.RoleName} / {x.ServiceGroup}";
                else return $"{x.RoleName} / {x.Service}";
            }).Distinct().ToList();

            var catalogueData = serviceCatalogueRoleData.Select(x => $"{x.RoleName} / {x.Portfolio} / {x.ServiceGroup} / {x.Service}").Distinct().ToList();

            var staffCountPerServiceGroup = this.GetStaffCountWithCache(KPIFixedColumn.ServiceGroup);
            var staffCountPerPortfolio = this.GetStaffCountWithCache(KPIFixedColumn.Portfolio);
            #endregion

            if (employee == null)
            {
                Log.Error("Unable to get employee data while generating user profile data. Email: {@email}", email);
                return (GetResponse<List<PermissionModel>>)response.NotAuthorized();
            }

            // Generate required employee data...
            var selectedEmployeeServiceGroup = staffCountPerServiceGroup.Where(x => serviceCatalogueRoleData.Any(y => y.ServiceGroup == x.Key)).OrderBy(z => z.Value).Select(t => t.Key).LastOrDefault();

            var selectedEmployeePortfolio = staffCountPerPortfolio.Where(x => serviceCatalogueRoleData.Any(y => y.Portfolio == x.Key)).OrderBy(z => z.Value).Select(t => t.Key).LastOrDefault();

            var permissionFactory = new PermissionFactory();

            #region AIDI Employees (Profile-1)
            if (employee.SubDepartmentCode == SubDept.AIDI)
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_1;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.USER_CATALOGUE_INFO,
                    new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));
            }
            #endregion

            #region Employees in Service Catalogue - HoEs (Profile-2)
            else if (serviceCatalogueRoleData.Any(x => x.RoleName == Roles.HeadOfEngineering) &&
                employee.Level != Levels.N_2
                )
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_2;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.KPI);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.KPI);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.USER_CATALOGUE_INFO,
                    new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_MAIN_FILTER,
                    new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" }, { "DEFAULT_VALUE", selectedEmployeePortfolio } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_GRAPHS,
                    new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_INTERVAL_FILTER,
                    new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_TABLE,
                    new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE_GROUP" } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_PORTFOLIO_FILTER,
                    new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", selectedEmployeePortfolio } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_SERVICE_GROUP_FILTER,
                    new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", "ALL" } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_TABLE,
                    new Dictionary<string, object>() { { "HIDE_KEY_RESULTS", true } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_INTERVAL_FILTER,
                    new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));
            }
            #endregion

            #region Non HoE Employees in Service Catalogue (Profile-3)
            else if (!serviceCatalogueRoleData.Any(x => x.RoleName == Roles.HeadOfEngineering) &&
                     !string.IsNullOrWhiteSpace(employee.RoleName) &&
                     employee.Level != Levels.N_2 &&
                     employee.Level != Levels.N_1
                 )
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_2;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.KPI);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.KPI);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.USER_CATALOGUE_INFO,
                   new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_MAIN_FILTER,
                    new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE_GROUP" }, { "DEFAULT_VALUE", selectedEmployeeServiceGroup } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_GRAPHS,
                    new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE_GROUP" } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_INTERVAL_FILTER,
                    new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_TABLE,
                    new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE" } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_PORTFOLIO_FILTER,
                    new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", selectedEmployeePortfolio } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_SERVICE_GROUP_FILTER,
                    new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", selectedEmployeeServiceGroup } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_TABLE,
                    new Dictionary<string, object>() { { "HIDE_KEY_RESULTS", true } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.OKR_INTERVAL_FILTER,
                    new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));
            }
            #endregion

            #region Multiple Segments (Profile-4) (Profile-7)
            else if ((employee.SubDepartmentCode == SubDept.Engineering && // Employees not in Service Catalogue (Profile-4)
                string.IsNullOrWhiteSpace(employee.RoleName) &&
                employee.Level != Levels.N_2)
                ||
                (employee.SubDepartmentCode == SubDept.SID) // SID (Profile-7)
                )
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_2;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.KPI);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.KPI);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.USER_CATALOGUE_INFO,
                   new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_MAIN_FILTER,
                    new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" }, { "DEFAULT_VALUE", "EBP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_GRAPHS,
                   new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_INTERVAL_FILTER,
                   new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_TABLE,
                   new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE_GROUP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_PORTFOLIO_FILTER,
                   new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", "EBP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_SERVICE_GROUP_FILTER,
                   new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", "ALL" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_TABLE,
                   new Dictionary<string, object>() { { "HIDE_KEY_RESULTS", true } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_INTERVAL_FILTER,
                   new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));
            }
            #endregion

            #region Multiple Segments (Profile-5) (Profile-6) (BI-Team)
            else if ((employee.DepartmentCode == Dept.GSD && // SVPs & CTO (Profile-5)
                 (employee.Level == Levels.N_2 || employee.Level == Levels.N_1))
                 ||
                 (employee.SubDepartmentCode == SubDept.EngineeringOperations && // Engineering Operations Team (Profile-6)
                 employee.Level != Levels.N_2)
                 ||
                 permissionFactory.IsMemberOfBusinessIntelligenceTeam(employee.WorkEmail) // BI-Team
                )
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.FullyGrantedProfile;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.KPI);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.KPI);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.USER_CATALOGUE_INFO,
                   new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));

                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.KPI_MAIN_FILTER,
                    new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" }, { "DEFAULT_VALUE", "EBP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_GRAPHS,
                   new Dictionary<string, object>() { { "DATA_TYPE", "PORTFOLIO" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_INTERVAL_FILTER,
                   new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.KPI_TABLE,
                   new Dictionary<string, object>() { { "DATA_TYPE", "SERVICE_GROUP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_PORTFOLIO_FILTER,
                   new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", "EBP" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_SERVICE_GROUP_FILTER,
                   new Dictionary<string, object>() { { "HAS_ALL_OPTION", true }, { "DEFAULT_VALUE", "ALL" } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_TABLE,
                   new Dictionary<string, object>() { { "HIDE_KEY_RESULTS", true } }));

                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.OKR_INTERVAL_FILTER,
                   new Dictionary<string, object>() { { "DEFAULT_VALUE", this.CalculateCurrentQuarter() } }));
            }
            #endregion

            #region France & Spain Employees (Profile-7)
            else if (employee.DepartmentCode == Dept.GSD &&
                (employee.Country == Countries.France || employee.Country == Countries.Spain))
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_3;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.USER_CATALOGUE_INFO,
                    new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));
            }
            #endregion

            #region Others (Profile-8)
            else
            {
                var menuPermissions = permissionFactory.NavigationMenuFactory.Profile_1;

                // Landing Page...
                result.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);
                menuPermissions.SetLandingPage(Constants.Authorization.LandingPages.MyCareer);

                // Menus...
                result.Add(new PermissionModel(
                   PermissionType.NavigationMenu,
                   Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE,
                   new KeyValuePair<string, object>(
                       "NAVIGATION_MENU",
                        menuPermissions)));

                // Components...
                result.Add(new PermissionModel(
                    PermissionType.Component,
                    Components.USER_CATALOGUE_INFO,
                    new Dictionary<string, object>() { { "DISPLAY_NAME", employee.EmployeeName }, { "JOB_TITLE", employee.BusinessTitle }, { "SUPERVISOR", employee.FirstSupervisor }, { "CATALOGUE_DATA", catalogueData }, { "DISPLAY_CATALOGUE_INFO", displayCatalogueInfo } }));
            }
            #endregion

            #region Additional Features
            if (permissionFactory.CanImpersonate(email))
            {
                result.Add(new PermissionModel(
                   PermissionType.Component,
                   Components.PORTAL_IMPERSONATION_COMPONENT));
            }

            if (permissionFactory.IsTimeKeepingAdmin(email))
            {
                var permittedMenus = (List<MenuPermissionModel>)result.Where(x => x.Type == PermissionType.NavigationMenu).Single().Props.Where(x => x.Key == Constants.Authorization.NAVIGATION_MENU_COMPONENT_CODE).Single().Value;

                permittedMenus.AddRange(new List<MenuPermissionModel>()
                {
                   permissionFactory.NavigationMenuFactory.TIMEKEEPING_IN_PERCENT_PERIOD_MANAGEMENT,
                   permissionFactory.NavigationMenuFactory.TIMEKEEPING_IN_PERCENT_ADMINISTRATION
                });
            }
            #endregion

            response.Success();
            response.Data = result;
            return response;
        }

        private string CalculateCurrentQuarter()
        {
            var numberOfDaysForBeingCurrentQuarter = 30;
            var originDate = DateTime.UtcNow.Date.AddDays(-1 * numberOfDaysForBeingCurrentQuarter);
            return $"{originDate.Year} Q{Math.Ceiling(originDate.Month / 3M)}";
        }

        #region Data Layer
        public async Task<List<ServiceCatalogueRoleDto>> GetServiceCatalogueRolesWithCache(string email)
        {
            List<ServiceCatalogueRoleDto> data = null;

            // Try Cache First...
            var cachekey = $"ServiceCatalogueRoles_{email}";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out data);

            if (!isFoundAtCache)
            {
                data = await _dbContext.ServiceCatalogueRoles.Where(x => x.EmailAddress.ToUpper() == email.ToUpper()).Select(x => new ServiceCatalogueRoleDto()
                {
                    RoleName = x.RoleName,
                    Portfolio = x.Portfolio,
                    ServiceGroup = x.ServiceGroup,
                    Service = x.Service
                }).ToListAsync();

                // Update Cache...
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Cache.Jira.LifetimeInHours));
            }

            return data;
        }

        public async Task<PaycomEmployeeFull> GetPaycomEmployeeFullDataWithCache(string email)
        {
            PaycomEmployeeFull data = null;

            // Try Cache First...
            var cachekey = $"PaycomEmployeeFull_{email}";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out data);

            if (!isFoundAtCache)
            {
                data = await _dbContext.PaycomEmployeeFulls.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == "WORKING" && x.WorkEmail.ToUpper() == email).FirstOrDefaultAsync();

                // Update Cache...
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Cache.Paycom.LifetimeInHours));
            }

            return data;
        }

        private Dictionary<string, int> GetStaffCountWithCache(KPIFixedColumn groupBy)
        {
            Dictionary<string, int> result = null;

            // Try Cache First...
            var cachekey = $"GetStaffCountPer{groupBy.ToString()}";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out result);

            if (isFoundAtCache)
                return result;

            // Then Fetch From DB...
            switch (groupBy)
            {
                case KPIFixedColumn.Portfolio:
                    result = _dbContext.ServiceCatalogueRoles
                        .Where(x => x.Portfolio != null)
                        .AsEnumerable()
                        .GroupBy(x => x.Portfolio)
                        .ToDictionary(k => k.Key, v => v.Count());
                    break;
                case KPIFixedColumn.ServiceGroup:
                    result = _dbContext.ServiceCatalogueRoles
                        .Where(x => x.ServiceGroup != null)
                        .AsEnumerable()
                        .GroupBy(x => x.ServiceGroup)
                        .ToDictionary(k => k.Key, v => v.Count());
                    break;
                case KPIFixedColumn.Service:
                    result = _dbContext.ServiceCatalogueRoles
                        .Where(x => x.Service != null)
                        .AsEnumerable()
                        .GroupBy(x => x.Service)
                        .ToDictionary(k => k.Key, v => v.Count());
                    break;
            }

            // Update Cache...
            _cacheProvider.Set(cachekey, result, TimeSpan.FromHours(this.JiraCacheDurationInHours));
            return result;
        }
        #endregion
    }
}