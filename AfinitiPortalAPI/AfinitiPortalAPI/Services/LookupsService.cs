using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.DTOs;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.services
{
    public interface ILookupsService
    {
        public Task<GetResponse<List<PaycomDepartments>>> GetPaycomDepartmentsInfo();
        public Task<GetResponse<List<ResourceModelRolesGroupDTO>>> GetGroups();
        public Task<GetResponse<List<SwitchProvidersDTO>>> GetSwitchProviders();
        public Task<GetResponse<List<SwitchPlatformsDTO>>> GetSwitchPlatforms();
        public Task<GetResponse<List<NewEmployeeSwitchKnowledgeDTO>>> GetSIDEmployees();
        public Task<GetResponse<List<RmRoleDTOTrimmed>>> GetRoles(string teamName);
        public Task<GetResponse<List<RmDepartmentDTO>>> GetDepartments();
        public Task<GetResponse<List<RmSubdepartmentDTO>>> GetSubDepartments();
        public Task<GetResponse<List<RmTeamDTO>>> GetTeams();
        public Task<GetResponse<List<TsJiraIssueCategoryDTO>>> GetJiraIssueCategories();
        public Task<GetResponse<List<TsJiraIssueTypeDTO>>> GetJiraIssueTypes();
        public Task<GetResponse<List<TsJiraIssueTypeDTO>>> GetHermesAccountsForJiraIssues();

    }
    public class LookupsService : ILookupsService
    {
        private readonly PortalDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cacheProvider;
        private readonly IOptions<AppSettings> _appSettings;
        public LookupsService(PortalDBContext dbContext, IMapper mapper, IMemoryCache cacheProvider, IOptions<AppSettings> appSettings, WebApiContext webApiContext)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheProvider = cacheProvider;
            _appSettings = appSettings;
        }
        public async Task<GetResponse<List<SwitchPlatformsDTO>>> GetSwitchPlatforms()
        {
            Log.Information("LookupsService: GetSwitchPlatforms Started");
            GetResponse<List<SwitchPlatformsDTO>> result = new();

            result.Data = await (from hermesOrg in _dbContext.HermesOrganizations
                                 join hermesSwitchPlatforms in _dbContext.HermesSwitchPlatforms on hermesOrg.Id equals hermesSwitchPlatforms.ProviderId
                                 select new SwitchPlatformsDTO()
                                 {
                                     //PlatformId1 = hermesOrg.Id,
                                     //PlatformName1 = hermesOrg.Name,
                                     PlatformId = hermesSwitchPlatforms.ProviderId ?? 0,
                                     PlatformName = hermesSwitchPlatforms.Name
                                 }).AsNoTracking().ToListAsync();

            Log.Information("LookupsService: GetSwitchPlatforms Completed");
            return result;
        }
        public async Task<GetResponse<List<SwitchProvidersDTO>>> GetSwitchProviders()
        {
            Log.Information("LookupsService: GetSwitchProviders Started");
            GetResponse<List<SwitchProvidersDTO>> result = new();
            result.Data = await _dbContext.HermesOrganizations.Where(x => x.Discriminator == "SwitchProvider").AsNoTracking().ProjectTo<SwitchProvidersDTO>(_mapper.ConfigurationProvider).ToListAsync();
            Log.Information("LookupsService: GetSwitchProviders Completed");
            return result;
        }
        public async Task<GetResponse<List<ResourceModelRolesGroupDTO>>> GetGroups()
        {
            Log.Information("LookupsService: GetGroups Started");
            GetResponse<List<ResourceModelRolesGroupDTO>> result = new();
            result.Data = await _dbContext.ResourceModelRolesGroups.AsNoTracking().ProjectTo<ResourceModelRolesGroupDTO>(_mapper.ConfigurationProvider).Where(x => x.IsActive == true).ToListAsync();
            Log.Information("LookupsService: GetGroups Completed");
            return result;
        }
        public async Task<GetResponse<List<PaycomDepartments>>> GetPaycomDepartmentsInfo()
        {
            Log.Information("LookupsService: GetPaycomDepartmentsInfo Started");
            GetResponse<List<PaycomDepartments>> result = new();
            var cachekey = "GetPaycomDepartmentsInfo";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out List<PaycomDepartments> data);
            if (!isFoundAtCache)
            {
                List<PaycomEmployeeFull> PaycomEmployeeDataFromDB = await GetPaycomEmployeeFullDataWithCache();
                data = PaycomEmployeeDataFromDB.AsQueryable().ProjectTo<PaycomDepartments>(_mapper.ConfigurationProvider).GroupBy(g => g.SubDepartmentCode).Select(grp => grp.FirstOrDefault()).OrderBy(o => o.SubDepartmentDisplayName).ToList();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            Log.Information("LookupsService: GetPaycomDepartmentsInfo Completed");
            result.Data = data;
            return result;
        }
        public async Task<List<PaycomEmployeeFull>> GetPaycomEmployeeFullDataWithCache()
        {
            var cachekey = $"PaycomEmployeeFull";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out List<PaycomEmployeeFull> data);
            if (!isFoundAtCache)
            {
                data = await _dbContext.PaycomEmployeeFulls.AsNoTracking().Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == "WORKING").ToListAsync();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            return data;
        }
        public async Task<List<PaycomEmployeeFull>> GetSIDEmployeesWithCache()
        {
            var cachekey = $"SIDEmployeeFull";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out List<PaycomEmployeeFull> data);
            if (!isFoundAtCache)
            {
                data = await _dbContext.PaycomEmployeeFulls.AsNoTracking().Where(x => x.ChangeStatus == "CURRENT STATUS" && x.SubDepartment == "SID" && x.WorkEmail != "").ToListAsync();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            return data;
        }
        public async Task<GetResponse<List<NewEmployeeSwitchKnowledgeDTO>>> GetSIDEmployees()
        {
            Log.Information("LookupsService: GetSIDEmployees Started");
            GetResponse<List<NewEmployeeSwitchKnowledgeDTO>> result = new();
            var cachekey = $"SIDEmployeeFull";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out List<NewEmployeeSwitchKnowledgeDTO> data);
            if (!isFoundAtCache)
            {
                data = await _dbContext.PaycomEmployeeFulls.AsNoTracking().Where(x => x.ChangeStatus == "CURRENT STATUS" && x.SubDepartment == "SID" && x.WorkEmail != "").ProjectTo<NewEmployeeSwitchKnowledgeDTO>(_mapper.ConfigurationProvider).ToListAsync();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            Log.Information("LookupsService: GetSIDEmployees Completed");
            result.Data = data;
            return result;
        }
        public async Task<GetResponse<List<RmRoleDTOTrimmed>>> GetRoles(string teamName)
        {
            Log.Information("LookupsService: GetRoles Started");
            GetResponse<List<RmRoleDTOTrimmed>> result = new();
            if (teamName == "SID")
            {
                result.Data = await (from roles in _dbContext.RmRoles.AsNoTracking()
                                     join depts in _dbContext.RmDepartments on roles.DepartmentId equals depts.Id
                                     join subDepts in _dbContext.RmSubdepartments on roles.SubdepartmentId equals subDepts.Id
                                     join teams in _dbContext.RmTeams on roles.TeamId equals teams.Id
                                     where roles.IsActive && depts.Name == "GSD" && subDepts.Name == "SID" && teams.Name == "SID"
                                     select new RmRoleDTOTrimmed()
                                     {
                                         Id = roles.Id,
                                         Name = roles.Name,
                                     }).ToListAsync();
            }
            else
                result.Data = await _dbContext.RmRoles.Where(x => x.IsActive == true).AsNoTracking().ProjectTo<RmRoleDTOTrimmed>(_mapper.ConfigurationProvider).ToListAsync();
            Log.Information("LookupsService: GetRoles Completed");
            return result;
        }
        public async Task<GetResponse<List<RmDepartmentDTO>>> GetDepartments()
        {
            Log.Information("LookupsService: GetDepartments Started");
            GetResponse<List<RmDepartmentDTO>> result = new();
            result.Data = await _dbContext.RmDepartments.AsNoTracking().ProjectTo<RmDepartmentDTO>(_mapper.ConfigurationProvider).Where(x => x.IsActive == true).ToListAsync();
            Log.Information("LookupsService: GetDepartments Completed");
            return result;
        }
        public async Task<GetResponse<List<RmSubdepartmentDTO>>> GetSubDepartments()
        {
            Log.Information("LookupsService: GetSubDepartments Started");
            GetResponse<List<RmSubdepartmentDTO>> result = new();
            result.Data = await _dbContext.RmSubdepartments.AsNoTracking().ProjectTo<RmSubdepartmentDTO>(_mapper.ConfigurationProvider).Where(x => x.IsActive == true).ToListAsync();
            Log.Information("LookupsService: GetSubDepartments Completed");
            return result;
        }
        public async Task<GetResponse<List<RmTeamDTO>>> GetTeams()
        {
            Log.Information("LookupsService: GetTeams Started");
            GetResponse<List<RmTeamDTO>> result = new();
            result.Data = await _dbContext.RmTeams.AsNoTracking().ProjectTo<RmTeamDTO>(_mapper.ConfigurationProvider).Where(x => x.IsActive == true).ToListAsync();
            Log.Information("LookupsService: GetTeams Completed");
            return result;
        }
        public async Task<GetResponse<List<TsJiraIssueCategoryDTO>>> GetJiraIssueCategories()
        {
            Log.Information("LookupsService: GetJiraIssueCategories Started");
            GetResponse<List<TsJiraIssueCategoryDTO>> result = new();
            result.Data = await _dbContext.TsJiraIssueCategories.Where(x => x.IsActive == true).AsNoTracking().ProjectTo<TsJiraIssueCategoryDTO>(_mapper.ConfigurationProvider).ToListAsync();
            Log.Information("LookupsService: GetJiraIssueCategories Completed");
            return result;
        }
        public async Task<GetResponse<List<TsJiraIssueTypeDTO>>> GetJiraIssueTypes()
        {
            Log.Information("LookupsService: GetJiraIssueTypes Started");
            GetResponse<List<TsJiraIssueTypeDTO>> result = new();
            result.Data = await _dbContext.TsJiraIssueTypes.AsNoTracking().Where(x => x.IsActive == true).ProjectTo<TsJiraIssueTypeDTO>(_mapper.ConfigurationProvider).ToListAsync();
            Log.Information("LookupsService: GetJiraIssueTypes Completed");
            return result;
        }

        public async Task<GetResponse<List<TsJiraIssueTypeDTO>>> GetHermesAccountsForJiraIssues()
        {
            Log.Information("LookupsService: GetHermesAccountsForJiraIssues Started");
            GetResponse<List<TsJiraIssueTypeDTO>> result = new();
            result.Data = await _dbContext.HermesOrganizations.AsNoTracking().Where(x => x.Discriminator == "Account" && x.Deleted == 0).ProjectTo<TsJiraIssueTypeDTO>(_mapper.ConfigurationProvider).ToListAsync();
            result.Data.ForEach(x => x.IssueCategoryId = 0);
            Log.Information("LookupsService: GetHermesAccountsForJiraIssues Completed");
            return result;
        }
    }
}