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
    public interface IOrganizationChartService
    {
        public Task<GetResponse<List<PaycomEmployeeFull_DTOFlat>>> GetUserOrgDataFlat(OrgChartRequestObj model);
        public Task<List<PaycomEmployeeFull_DTO>> GetUserOrgData(OrgChartRequestObj model);
        public Task<GetResponse<List<AfinitiUser>>> GetAllUsers();
    }

    public class OrganizationChartService : IOrganizationChartService
    {
        private List<PaycomEmployeeFull_DTOFlat> FlatHeadData = new();
        private static List<PaycomEmployeeFull_DTOFlat> FlatData = new();
        private readonly PortalDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cacheProvider;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly WebApiContext _webApiContext;

        public OrganizationChartService(PortalDBContext dbContext, IMapper mapper, IMemoryCache cacheProvider, IOptions<AppSettings> appSettings, WebApiContext webApiContext)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheProvider = cacheProvider;
            _appSettings = appSettings;
            _webApiContext = webApiContext;
        }


        private List<PaycomEmployeeFull_DTOFlat> GetHeadsData(List<PaycomEmployeeFull_DTOFlat> list, string Emp_Code, string headMainNodeLevel)
        {
            foreach (var item in list.Where(x => x.nodeId == Emp_Code))
            {
                if (list.Any(xx => xx.WorkEmail == item.FirstSupervisorWorkEmail))
                {
                    if (!FlatHeadData.Any(x => x.nodeId == item.nodeId))
                        FlatHeadData.Add(item);
                    Log.Information("UserService: GetUserOrgData WIP", Emp_Code);
                }
                else  /*If any InActive Head is there for any Active employee then from that node, no top tier of that employee will be added in his Org. Only his bottom tier will be displayed in this case*/
                {
                    if (!FlatHeadData.Any(x => x.nodeId == item.nodeId))
                    {
                        item.parentNodeId = null;
                        item.FirstSupervisorWorkEmail = null;
                        item.FirstSupervisorWorkStatus = null;
                        FlatHeadData.Add(item);
                        Log.Information("UserService: GetUserOrgData WIP", Emp_Code);
                        break;
                    }
                }
                GetHeadsData(list, item.parentNodeId, headMainNodeLevel);
            }
            //Make root null for  N-2 onwards level employees
            if (headMainNodeLevel != "N" && headMainNodeLevel != "N-1" && headMainNodeLevel != "N-2")
            {
                if (FlatHeadData.Any(x => x.Level == "N-2")) FlatHeadData.FirstOrDefault(x => x.Level == "N-2").parentNodeId = null;
                if (FlatHeadData.Any(x => x.Level == "N")) FlatHeadData.FirstOrDefault(x => x.Level == "N").SideChartFlag = true;
                if (FlatHeadData.Any(x => x.Level == "N-1")) FlatHeadData.FirstOrDefault(x => x.Level == "N-1").SideChartFlag = true;
            }
            return FlatHeadData;
        }

        private async Task<List<PaycomEmployeeFull_DTOFlat>> GerOrgChartDataOfUser(OrgChartRequestObj model)
        {
            List<PaycomEmployeeFull_DTOFlat> baseData = new();
            /*All Active Head Nodes till CEO node*/
            List<PaycomEmployeeFull_DTOFlat> headNodes = new();
            List<PaycomEmployeeFull> PaycomEmployeeDataFromDB = await GetPaycomEmployeeFullDataWithCache();
            var headMainNode = PaycomEmployeeDataFromDB.FirstOrDefault(filter => filter.WorkEmail == model.WorkEmail && filter.WorkStatus == "WORKING" && filter.ChangeStatus == "CURRENT STATUS");
            if (headMainNode != null)
            {
                var nodeIDs = new[]
                {
                    headMainNode.CeoCode,headMainNode.OneLevelBelowCeoCode, headMainNode.TwoLevelBelowCeoCode,headMainNode.ThreeLevelBelowCeoCode, headMainNode.FourLevelBelowCeoCode,
                    headMainNode.FiveLevelBelowCeoCode,headMainNode.SixLevelBelowCeoCode, headMainNode.SevenLevelBelowCeoCode,headMainNode.EigthLevelBelowCeoCode,headMainNode.NineLevelBelowCeoCode
                };
                headNodes = PaycomEmployeeDataFromDB.Where(value => value.ChangeStatus == "CURRENT STATUS" && value.WorkStatus == "WORKING" && nodeIDs.Contains(value.EmployeeCode)).AsQueryable()
                    .ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).ToList();
            }
            FlatHeadData = new();
            GetHeadsData(headNodes, headMainNode.EmployeeCode, headMainNode.Level);
            /*Head Nodes*/
            string position = headMainNode.Position;
            if (!model.FetchTillLastEdge)// Direct reportees only
            {
                if (FlatHeadData?.Count > 0)
                    baseData.AddRange(FlatHeadData);
                else // If no head nodes then add this user as parent node
                {
                    baseData.Add(PaycomEmployeeDataFromDB.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkEmail == model.WorkEmail.ToUpper()).AsQueryable()
    .ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).FirstOrDefault());
                    baseData[0].parentNodeId = null;
                    baseData[0].FirstSupervisorWorkEmail = null;
                    baseData[0].FirstSupervisorWorkStatus = null;
                }
                baseData.AddRange(PaycomEmployeeDataFromDB.Where(x =>
               x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == model.WorkStatus && x.FirstSupervisorWorkEmail == model.WorkEmail &&
               x.FirstSupervisorWorkEmail != null && x.FirstSupervisorWorkEmail != "" && x.FirstSupervisorCode != "" && x.FirstSupervisorCode != null
                ).AsQueryable().ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).ToList());
            }
            else if (position == "Chief Executive Officer" && model.FetchTillLastEdge)// All data for CEO
            {
                baseData.Add(PaycomEmployeeDataFromDB.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkEmail == model.WorkEmail.ToUpper()).AsQueryable()
                  .ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).FirstOrDefault());
                baseData[0].parentNodeId = null;
                baseData.AddRange(PaycomEmployeeDataFromDB.Where(x => x.ChangeStatus == "CURRENT STATUS"
               && x.WorkStatus == "WORKING" && x.FirstSupervisorWorkStatus == "WORKING" &&
               x.FirstSupervisorWorkEmail != null && x.FirstSupervisorWorkEmail != "" && x.FirstSupervisorCode != "" && x.FirstSupervisorCode != null
               && x.WorkEmail.ToUpper() != model.WorkEmail.ToUpper())
                  .AsQueryable().ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).ToList());

                //Active employees with disable manager- WORKING Exception CASE case 
                List<PaycomEmployeeFull_DTOFlat> baseDataActiveEmployeeExceptionCase = new();
                List<PaycomEmployeeFull_DTOFlat> GoneManagers = new();
                List<string> emails = new();
                if (model.WorkStatus == "WORKING")
                {
                    baseDataActiveEmployeeExceptionCase.AddRange(PaycomEmployeeDataFromDB.Where(z => z.ChangeStatus == "CURRENT STATUS"
                   && z.WorkStatus == "WORKING" && z.FirstSupervisorWorkStatus != "WORKING" && z.FirstSupervisorWorkEmail != null &&
                   z.FirstSupervisorWorkEmail != "" && z.FirstSupervisorCode != "" && z.FirstSupervisorCode != null)
                   .AsQueryable().ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).ToList());

                    baseDataActiveEmployeeExceptionCase.Select(s => { s.FirstSupervisorWorkStatus = "WORKING EXCEPTION"; return s; }).ToList();
                    emails = baseDataActiveEmployeeExceptionCase.Where(s => s.FirstSupervisorWorkStatus == "WORKING EXCEPTION").Select(x => x.FirstSupervisorWorkEmail).Distinct().ToList();
                }
                foreach (var item in baseDataActiveEmployeeExceptionCase)
                {
                    if (!baseData.Any(s => s.nodeId == item.nodeId))
                        baseData.Add(item);
                }
                foreach (var item in emails)
                {
                    if (!baseData.Any(s => s.WorkEmail == item))
                    {
                        PaycomEmployeeFull_DTOFlat GoneManager = PaycomEmployeeDataFromDB.Where(z => z.WorkEmail == item).AsQueryable().ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).FirstOrDefault();
                        GoneManager.WorkStatus = "WORKING EXCEPTION";
                        baseData.Add(GoneManager);
                    }
                }
            }
            else if (position != "Chief Executive Officer" && model.FetchTillLastEdge)// till last edge data except for Zia
            {
                List<PaycomEmployeeFull_DTOFlat> mainData = new();
                FlatData.Clear();
                mainData = PaycomEmployeeDataFromDB.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == "WORKING"
              && x.FirstSupervisorWorkStatus == "WORKING" && !string.IsNullOrEmpty(x.FirstSupervisorWorkEmail) && !string.IsNullOrEmpty(x.FirstSupervisorCode)
                ).AsQueryable().ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).ToList(); // All data
                List<Task> gTaskList = new();
                gTaskList.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        FlatToHierarchy_Flat(mainData, headMainNode.EmployeeCode);
                        Log.Information("UserService: GetUserOrgData done");
                    }
                    catch (Exception e)
                    {
                        Log.Error("UserService: GetUserOrgData error", e);
                    }
                }).ContinueWith(m =>
                {
                    if (m.Status == TaskStatus.Faulted)
                    {
                        if (m.Exception != null)
                            Log.Error("UserService: GetUserOrgData error", m.Exception.GetBaseException());
                    }
                    else if (m.Status == TaskStatus.RanToCompletion)
                    {
                        baseData.Clear();

                        if (FlatHeadData?.Count > 0)
                            baseData.AddRange(FlatHeadData);
                        else
                        {
                            baseData.Add(PaycomEmployeeDataFromDB.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkEmail == model.WorkEmail.ToUpper()).AsQueryable()
.ProjectTo<PaycomEmployeeFull_DTOFlat>(_mapper.ConfigurationProvider).FirstOrDefault());
                            baseData[0].parentNodeId = null;
                            baseData[0].FirstSupervisorWorkEmail = null;
                            baseData[0].FirstSupervisorWorkStatus = null;
                        }

                        baseData.AddRange(FlatData);
                    }
                }, TaskContinuationOptions.None));
                Task.WhenAll(gTaskList.ToArray()).Wait(); //Either add .Wait() or add await with WhenAll() ==> both are same
                Log.Information("UserService: GetUserOrgData WIP");
            }
            ///*Service Catalogue for employees*/
            //List<Task> ServiceTaskList = new List<Task>();
            //ServiceTaskList.Add(Task.Factory.StartNew(() =>
            //{
            //    try
            //    {
            //        var portfoliosList = _dbContext.ServiceCatalogueRoles.ToList();
            //        baseData.Select(item =>
            //        {
            //            item.Portfolio = string.Join(", ", portfoliosList.Where(s => s.EmailAddress.ToLower() == item.WorkEmail.ToLower()).Select(x => x.Portfolio).Distinct().ToArray());
            //            item.Service = string.Join(", ", portfoliosList.Where(s => s.EmailAddress.ToLower() == item.WorkEmail.ToLower()).Select(x => x.Service).Distinct().ToArray());
            //            item.ServiceGroup = string.Join(", ", portfoliosList.Where(s => s.EmailAddress.ToLower() == item.WorkEmail.ToLower()).Select(x => x.ServiceGroup).Distinct().ToArray());
            //            item.Role = string.Join(", ", portfoliosList.Where(s => s.EmailAddress.ToLower() == item.WorkEmail.ToLower()).Select(x => x.RoleName).Distinct().ToArray());
            //            return item;
            //        }).ToList();
            //    }
            //    catch (Exception e)
            //    {
            //        Log.Error("UserService: GetUserOrgData:Service Catalogue error", e);

            //    }
            //}));
            //Task.WhenAll(ServiceTaskList.ToArray()).Wait();
            Log.Information("UserService: GetUserOrgData Completed");
            return baseData;
        }
        public async Task<GetResponse<List<PaycomEmployeeFull_DTOFlat>>> GetUserOrgDataFlat(OrgChartRequestObj model)
        {
            GetResponse<List<PaycomEmployeeFull_DTOFlat>> result = new GetResponse<List<PaycomEmployeeFull_DTOFlat>>();
            Log.Information("UserService: GetUserOrgData Started");
            List<PaycomEmployeeFull_DTOFlat> baseData = new();
            model.WorkEmail = model.WorkEmail.Trim().ToUpper();
            var cachekey = $"OrgChart_{model.WorkEmail}_{model.FetchTillLastEdge}";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out baseData);
            if (!isFoundAtCache)
            {
                baseData = await GerOrgChartDataOfUser(model);
                _cacheProvider.Set(cachekey, baseData, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            result.Data = baseData;
            return result;
        }

        private static List<PaycomEmployeeFull_DTOFlat> FlatToHierarchy_Flat(List<PaycomEmployeeFull_DTOFlat> list, string EmpLineMgr_Code)
        {
            foreach (var item in list.Where(x => x.parentNodeId == EmpLineMgr_Code))
            {
                if (!FlatData.Any(x => x.nodeId == item.nodeId))
                    FlatData.Add(item);
                Log.Information("UserService: GetUserOrgData WIP", EmpLineMgr_Code);

                FlatToHierarchy_Flat(list, item.nodeId);
            }
            return FlatData;
        }

        public async Task<List<PaycomEmployeeFull_DTO>> GetUserOrgData(OrgChartRequestObj model)
        {
            Log.Information("UserService: GetUserOrgData Started");
            List<PaycomEmployeeFull_DTO> baseData = new();
            List<PaycomEmployeeFull_DTO> ResponseData = new();

            baseData = await _dbContext.PaycomEmployeeFulls.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == model.WorkStatus)
            .ProjectTo<PaycomEmployeeFull_DTO>(_mapper.ConfigurationProvider).ToListAsync();

            Log.Information("UserService: GetUserOrgData InProgress");
            if (string.IsNullOrEmpty(model.WorkEmail))
                ResponseData = FlatToHierarchyLINQ(baseData, null, model.FetchTillLastEdge);
            else
                ResponseData = FlatToHierarchyLINQ(baseData, model.WorkEmail?.ToUpper(), model.FetchTillLastEdge);

            List<PaycomEmployeeFull_DTO> RootResponeObj = new()
            {
                _dbContext.PaycomEmployeeFulls.Where(x => x.WorkEmail == model.WorkEmail.ToUpper())
            .ProjectTo<PaycomEmployeeFull_DTO>(_mapper.ConfigurationProvider).FirstOrDefault()
                //baseData.FirstOrDefault(z => z.WorkEmail == model.WorkEmail?.ToUpper())
            };
            RootResponeObj[0].TeamOrgData = ResponseData;
            Log.Information("UserService: GetUserOrgData Completed");
            return RootResponeObj;
        }

        private static List<PaycomEmployeeFull_DTO> FlatToHierarchyLINQ(List<PaycomEmployeeFull_DTO> list, string parentUserEmailId, bool fetchTillLastEdge)
        {
            List<PaycomEmployeeFull_DTO> Headdata = new();
            List<PaycomEmployeeFull_DTO> TeamOrgData = new();
            if (fetchTillLastEdge)
            {
                return (from item in list
                        where item.FirstSupervisorWorkEmail?.ToUpper() == parentUserEmailId
                        select new PaycomEmployeeFull_DTO
                        {
                            Gender = item?.Gender,
                            EmployeeName = item?.EmployeeName,
                            EmployeeCode = item?.EmployeeCode,
                            PositionTitle = item?.PositionTitle,
                            WorkEmail = item?.WorkEmail,
                            FirstSupervisorWorkEmail = item?.FirstSupervisorWorkEmail,
                            FirstSupervisorCode = item?.FirstSupervisorCode,
                            TeamOrgData = FlatToHierarchyLINQ(list, item?.WorkEmail?.ToUpper(), fetchTillLastEdge)
                        }
                         ).ToList();
            }
            else
            {
                return (from item in list
                        where item.FirstSupervisorWorkEmail?.ToUpper() == parentUserEmailId
                        select new PaycomEmployeeFull_DTO
                        {
                            Gender = item?.Gender,
                            EmployeeName = item?.EmployeeName,
                            EmployeeCode = item?.EmployeeCode,
                            PositionTitle = item?.PositionTitle,
                            WorkEmail = item?.WorkEmail,
                            FirstSupervisorWorkEmail = item?.FirstSupervisorWorkEmail,
                            FirstSupervisorCode = item?.FirstSupervisorCode,
                            TeamOrgData = TeamOrgData
                        }).ToList();
            }
        }

        public async Task<GetResponse<List<AfinitiUser>>> GetAllUsers()
        {
            Log.Information("UserService: GetAllUsers Started");
            GetResponse<List<AfinitiUser>> result = new GetResponse<List<AfinitiUser>>();
            List<AfinitiUser> data = null;
            var cachekey = "GetAllUsers";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out data);
            if (!isFoundAtCache)
            {
                List<PaycomEmployeeFull> PaycomEmployeeDataFromDB = await GetPaycomEmployeeFullDataWithCache();
                data = PaycomEmployeeDataFromDB.Where(z => z.WorkEmail != "").AsQueryable().ProjectTo<AfinitiUser>(_mapper.ConfigurationProvider).Distinct().OrderBy(x => x.WorkEmail).ToList();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            Log.Information("UserService: GetAllUsers Completed");
            result.Data = data;
            return result;
        }

        public async Task<List<PaycomEmployeeFull>> GetPaycomEmployeeFullDataWithCache()
        {
            var cachekey = $"PaycomEmployeeFull";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out List<PaycomEmployeeFull> data);
            if (!isFoundAtCache)
            {
                data = await _dbContext.PaycomEmployeeFulls.Where(x => x.ChangeStatus == "CURRENT STATUS" && x.WorkStatus == "WORKING").ToListAsync();
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(_appSettings.Value.Cache.Paycom.LifetimeInHours));
            }
            return data;
        }
    }
}