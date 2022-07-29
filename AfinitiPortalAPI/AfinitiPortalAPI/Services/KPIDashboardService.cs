using AfinitiPortalAPI.Services.Base;
using AfinitiPortalAPI.Shared;
using AfinitiPortalAPI.Shared.DTOs;
using AfinitiPortalAPI.Shared.DTOs.KPIDashboard;
using AfinitiPortalAPI.Shared.Enums;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AfinitiPortalAPI.Data.Extensions;
using Roles = AfinitiPortalAPI.Shared.Constants.Paycom.RoleNames;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;

namespace AfinitiPortalAPI.services
{
    public interface IKPIDashboardService
    {
        Task<GetChartDataResponse> GetQualityChartData(GetChartDataRequest request, bool isTableData);
        Task<GetChartDataResponse> GetDeliverySpeedChartData(GetChartDataRequest request, bool isTableData);
        Task<GetChartDataResponse> GetProductionQualityChartData(GetChartDataRequest request, bool isTableData);
        Task<GetBulkChartDataResponse> GetBulkChartData(GetBulkChartDataRequest request);
        Task<GetEmployeeServiceCatalogueInfoResponse> GetEmployeeServiceCatalogueInfo(string email);
        Task<GetResponse<List<OKRTableItem>>> GetOKRTableData(string portfolio, string serviceGroup, string service, string interval, string email);
        Task<GetResponse<List<string>>> GetOKRIntervals();
        Task<GetResponse<List<KPIComboDataDto>>> GetComboDataForKPI(KPIComboboxType comboboxType);
        Task<GetComboDataForOkrResponse> GetComboDataForOKR(string email = null);
    }

    public class KPIDashboardService : BaseService, IKPIDashboardService
    {
        public KPIDashboardService(PortalDBContext dbContext, IMemoryCache cacheProvider, WebApiContext webApiContext, IOptions<AppSettings> configuration)
            : base(dbContext, cacheProvider, webApiContext, configuration)
        { }

        public async Task<GetChartDataResponse> GetQualityChartData(GetChartDataRequest request, bool isTableData)
        {
            var result = new GetChartDataResponse();
            try
            {
                var data = await this.GetKpiBackendPortalsWithCache();

                // Prepare Response...
                result.FixedBy = request.FixedBy.ToString();

                switch (request.FixedBy)
                {
                    case KPIFixedColumn.Portfolio:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.QualityPoL6wAvg, // Only Chart Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Portfolio = x.Portfolio
                            })
                            .Distinct()
                            .GroupBy(x => x.Portfolio)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.ServiceGroup:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = isTableData ? x.QualityIntervalSg : x.QualitySgL6wAvg, // May be both Graph or Table...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                ServiceGroup = x.ServiceGroup
                            })
                            .Distinct()
                            .GroupBy(x => x.ServiceGroup)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.Service:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.QualityIntervalSe, // Only Table Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Service = x.Service
                            })
                            .Distinct()
                            .GroupBy(x => x.Service)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unable to respond to request. Method: {@method} | Ex: {@ex}", "GetQualityChartData", ex);
                result.Failure();
            }
            return result;
        }

        public async Task<GetChartDataResponse> GetDeliverySpeedChartData(GetChartDataRequest request, bool isTableData)
        {
            var result = new GetChartDataResponse();

            try
            {
                var data = await this.GetKpiBackendPortalsWithCache();

                // Prepare Response...
                result.FixedBy = request.FixedBy.ToString();

                switch (request.FixedBy)
                {
                    case KPIFixedColumn.Portfolio:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.DeliverySpeedPoL6wAvg, // Only Chart Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Portfolio = x.Portfolio
                            })
                            .Distinct()
                            .GroupBy(x => x.Portfolio)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.ServiceGroup:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = isTableData ? x.DeliverySpeedIntervalSg : x.DeliverySpeedSgL6wAvg, // May be both Graph or Table...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                ServiceGroup = x.ServiceGroup
                            })
                            .Distinct()
                            .GroupBy(x => x.ServiceGroup)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.Service:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.DeliverySpeedIntervalSe, // Only Table Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Service = x.Service
                            })
                            .Distinct()
                            .GroupBy(x => x.Service)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unable to respond to request. Method: {@method} | Ex: {@ex}", "GetQualityChartData", ex);
                result.Failure();
            }
            return result;
        }

        public async Task<GetChartDataResponse> GetProductionQualityChartData(GetChartDataRequest request, bool isTableData)
        {
            var result = new GetChartDataResponse();

            try
            {
                var data = await this.GetKpiBackendPortalsWithCache();

                // Prepare Response...
                result.FixedBy = request.FixedBy.ToString();

                switch (request.FixedBy)
                {
                    case KPIFixedColumn.Portfolio:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.ProdQualityPoL6wAvg, // Only Chart Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Portfolio = x.Portfolio
                            })
                            .Distinct()
                            .GroupBy(x => x.Portfolio)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.ServiceGroup:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = isTableData ? x.ProdQualityIntervalSg : x.ProdQualitySgL6wAvg, // May be both Graph or Table...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                ServiceGroup = x.ServiceGroup
                            })
                            .Distinct()
                            .GroupBy(x => x.ServiceGroup)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;

                    case KPIFixedColumn.Service:
                        result.Data = data
                            .Where(x => x.Interval == request.Interval)
                            .Select(x => new
                            {
                                Interval = x.Interval,
                                MetricValue = x.ProdQualityIntervalSe, // Only Table Data...
                                StartOfWeek = x.StartOfWeekDate,
                                EndOfWeek = x.EndOfWeekDate,
                                Service = x.Service
                            })
                            .Distinct()
                            .GroupBy(x => x.Service)
                            .Select(x => new GetChartDataResponseItem()
                            {
                                FixedValue = x.Key,
                                Metrics = x.OrderBy(l => l.StartOfWeek.Value).Select(z => new GetChartDataResponseMetricItem()
                                {
                                    Date = z.StartOfWeek.Value.ToString(Constants.Api.ResponseDateFormat),
                                    MetricValue = Math.Round((decimal)(z.MetricValue.HasValue ? z.MetricValue.Value : 0), 2)
                                }).ToList()
                            }).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unable to respond to request. Method: {@method} | Ex: {@ex}", "GetQualityChartData", ex);
                result.Failure();
            }
            return result;
        }

        public async Task<GetBulkChartDataResponse> GetBulkChartData(GetBulkChartDataRequest request)
        {
            // Get All 3 Graphs Data...
            var qualityData = await this.GetQualityChartData(request.ToChartDataGraphRequest(), false);
            var deliverySpeedData = await this.GetDeliverySpeedChartData(request.ToChartDataGraphRequest(), false);
            var productionQualityData = await this.GetProductionQualityChartData(request.ToChartDataGraphRequest(), false);

            // Get Table Data...
            var qualityTableData = await this.GetQualityChartData(request.ToChartDatTableRequest(), true);
            var deliverySpeedTableData = await this.GetDeliverySpeedChartData(request.ToChartDatTableRequest(), true);
            var productionQualityTableData = await this.GetProductionQualityChartData(request.ToChartDatTableRequest(), true);

            // Prepare Result...
            var result = new GetBulkChartDataResponse();

            result.GraphsFixedBy = request.GraphsFixedBy.ToString();
            result.TableFixedBy = request.TableFixedBy.ToString();
            result.Interval = request.Interval.ToString();

            // Set Graphs...
            result.QualityData = qualityData.Data;
            result.DeliverySpeedData = deliverySpeedData.Data;
            result.ProductionQualityData = productionQualityData.Data;

            // Set Table...
            result.TableData.Data = qualityTableData.Data.Select(x => new GetTableDataResponseItem() { FixedValue = x.FixedValue, QualityMetric = Math.Round(x.Metrics.Average(x => x.MetricValue), 2) }).ToList();

            foreach (var x in deliverySpeedTableData.Data)
            {
                var datum = Math.Round(x.Metrics.Average(x => x.MetricValue), 2);
                var item = result.TableData.Data.Where(y => y.FixedValue == x.FixedValue).FirstOrDefault();

                if (item != null)
                    item.DeliverySpeedMetric = datum;
            }

            foreach (var x in productionQualityTableData.Data)
            {
                var datum = Math.Round(x.Metrics.Average(x => x.MetricValue), 2);
                var item = result.TableData.Data.Where(y => y.FixedValue == x.FixedValue).FirstOrDefault();

                if (item != null)
                    item.ProductionQualityMetric = datum;
            }

            #region Getting Target Scores
            var now = DateTime.UtcNow;
            var targetScores = _dbContext.KpiTargetScores.Where(x => now >= x.StartDate && now <= x.EndDate).FirstOrDefault();

            if (targetScores != null)
            {
                result.TableData.QualityTargetScore = targetScores.Quality;
                result.TableData.DeliverySpeedTargetScore = targetScores.DeliverySpeed;
                result.TableData.ProductionQualityTargetScore = targetScores.ProductionQuality;
            }
            #endregion

            #region Set 'FilterTerm'
            var serviceCatalogueLookupData = await _dbContext.ServiceCatalogueLookups.ToListAsync();

            foreach (var item in result.TableData.Data)
            {
                switch (request.GraphsFixedBy)
                {
                    case KPIFixedColumn.Portfolio:
                        switch (request.TableFixedBy)
                        {
                            case KPIFixedColumn.ServiceGroup:
                                item.FilterTerm = serviceCatalogueLookupData.Where(x => x.ServiceGroup == item.FixedValue).Select(x => x.Portfolio).FirstOrDefault();
                                break;
                            case KPIFixedColumn.Service:
                                item.FilterTerm = serviceCatalogueLookupData.Where(x => x.Service == item.FixedValue).Select(x => x.Portfolio).FirstOrDefault();
                                break;
                        }
                        break;
                    case KPIFixedColumn.ServiceGroup:
                        item.FilterTerm = serviceCatalogueLookupData.Where(x => x.Service == item.FixedValue).Select(x => x.ServiceGroup).FirstOrDefault();
                        break;
                }
            }
            #endregion

            return result;
        }

        public async Task<GetEmployeeServiceCatalogueInfoResponse> GetEmployeeServiceCatalogueInfo(string email)
        {
            var result = new GetEmployeeServiceCatalogueInfoResponse();

            try
            {
                var data = await this.GetServiceCatalogueRolesWithCache(email);

                result.Email = email;
                result.Data = data.Select(x => new GetEmployeeServiceCatalogueInfoResponseItem()
                {
                    Role = x.RoleName,
                    Portfolio = x.Portfolio,
                    ServiceGroup = x.ServiceGroup,
                    Service = x.Service,
                    PortfolioUrl = string.Empty,
                    ServiceGroupUrl = string.Empty,
                    ServiceUrl = string.Empty
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("GetEmployeeServiceCatalogueInfo() failed. Ex: {@ex}", ex);
                result.Failure();
            }

            return result;
        }

        public async Task<GetResponse<List<OKRTableItem>>> GetOKRTableData(string portfolio, string serviceGroup, string service, string interval, string email)
        {
            var result = new GetResponse<List<OKRTableItem>>();

            // Get All Data...
            var okrData = await this.GetPortalOkrBackendsWithCache();

            // Filter for Interval...
            okrData = okrData.Where(x => x.Interval == interval).ToList();

            // Filter for Email...
            if (!string.IsNullOrWhiteSpace(email))
            {
                List<ServiceCatalogueRoleDto> serviceCatalogueRolesOfUser = await this.GetServiceCatalogueRolesWithCache(email);

                okrData = serviceCatalogueRolesOfUser.Select(role =>
                {
                    if (role.RoleName == Roles.HeadOfEngineering)
                    {
                        return okrData.Where(okr => (role.Service == okr.Service && okr.Service != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_INHERITED) || (okr.Portfolio == role.Portfolio && okr.Portfolio != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_PORTFOLIO) || (okr.ServiceGroup == role.ServiceGroup && okr.ServiceGroup != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_SERVICEGROUP)).ToList();
                    }
                    else if (role.RoleName == Roles.ServiceGroupLead || role.RoleName == Roles.ServiceGroupArchitect || role.RoleName == Roles.ServiceGroupManager)
                    {
                        return okrData.Where(okr => (role.Service == okr.Service && okr.Service != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_INHERITED) || (okr.ServiceGroup == role.ServiceGroup && okr.ServiceGroup != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_SERVICEGROUP)).ToList();
                    }
                    else
                    {
                        return okrData.Where(okr => role.Service == okr.Service && okr.Service != null && okr.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_INHERITED).ToList();
                    }
                }).SelectMany(x => x).Distinct().ToList();
            }

            // Portfolio Filter...
            var portfolioOkrs = string.IsNullOrWhiteSpace(portfolio) ? okrData : okrData.Where(x => x.Portfolio != null && x.Portfolio == portfolio).ToList();

            // ServiceGroup Filter...
            var serviceGroupOkrs = string.IsNullOrWhiteSpace(serviceGroup) ? portfolioOkrs : portfolioOkrs.Where(x => x.ServiceGroup != null && x.ServiceGroup == serviceGroup).ToList();

            // Service Filter...
            var serviceOkrs = string.IsNullOrWhiteSpace(service) ? serviceGroupOkrs : serviceGroupOkrs.Where(x => x.Service != null && x.Service == service).ToList();

            // Append data to result...
            var temp1 = portfolioOkrs.Where(x => x.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_PORTFOLIO).ToOkrTableItemForPortfolio();
            var temp2 = serviceGroupOkrs.Where(x => x.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_SERVICEGROUP).ToOkrTableItemForServiceGroup();
            var temp3 = serviceOkrs.Where(x => x.OkrLevel == Constants.DB.dbo.PORTAL_OKR_BACKEND_OKR_LEVEL_INHERITED)?.ToOkrTableItemForService();
            result.Data.AddRange(temp1);
            result.Data.AddRange(temp2);
            result.Data.AddRange(temp3);

            return result;
        }

        public async Task<GetResponse<List<string>>> GetOKRIntervals()
        {
            var result = new GetResponse<List<string>>();
            var okrProjects = await this.GetPortalOkrBackendsWithCache();
            result.Data = okrProjects.GroupBy(x => x.Interval).Select(x => x.Key).Distinct().OrderBy(x => x).ToList();
            return result;
        }

        public async Task<GetComboDataForOkrResponse> GetComboDataForOKR(string email = null)
        {
            var result = new GetComboDataForOkrResponse();


            var data = await this.GetPortalOkrBackendsWithCache();

            if (!string.IsNullOrWhiteSpace(email)) // Only related with employee...
            {
                var serviceCatalogueRolesOfUser = await this.GetServiceCatalogueRolesWithCache(email);

                data = data.Where(okr =>
                    string.IsNullOrWhiteSpace(email) ? true : (
                        serviceCatalogueRolesOfUser.Any(x => x.Service == okr.Service) && okr.Service != null) ||
                        serviceCatalogueRolesOfUser.Any(x => x.ServiceGroup == okr.ServiceGroup) && okr.ServiceGroup != null ||
                        serviceCatalogueRolesOfUser.Any(x => x.Portfolio == okr.Portfolio) && okr.Portfolio != null
                    ).ToList();
            }

            result.PortfolioFilterData = data.Where(x => x.Portfolio != null).GroupBy(x => x.Portfolio).Select(x => new GetComboDataForOkrResponsePortfolioResponseItem() { Portfolio = x.Key }).ToList();
            result.ServiceGroupFilterData = data.Where(x => x.Portfolio != null && x.ServiceGroup != null).GroupBy(x => new { x.Portfolio, x.ServiceGroup }).Select(x => new GetComboDataForOkrResponseServiceGroupResponseItem() { Portfolio = x.Key.Portfolio, ServiceGroup = x.Key.ServiceGroup }).ToList();
            result.ServiceFilterData = data.Where(x => x.Portfolio != null && x.ServiceGroup != null && x.Service != null).GroupBy(x => new { x.Portfolio, x.ServiceGroup, x.Service }).Select(x => new GetComboDataForOkrResponseServiceResponseItem() { Portfolio = x.Key.Portfolio, ServiceGroup = x.Key.ServiceGroup, Service = x.Key.Service }).ToList();

            return result;
        }

        public async Task<GetResponse<List<KPIComboDataDto>>> GetComboDataForKPI(KPIComboboxType comboboxType)
        {
            var result = new GetResponse<List<KPIComboDataDto>>();
            var data = await this.GetKpiBackendPortalsWithCache();

            // Mega Environment
            var megaPortfolios = new List<string>() { "Mega Omni", "Mega Cloud", "Mega Voice" };
            var megaServiceGroups = data.Where(x => megaPortfolios.Contains(x.Portfolio)).Select(x => x.ServiceGroup).ToList();
            var megaServices = data.Where(x => megaPortfolios.Contains(x.Portfolio)).Select(x => x.Service).ToList();

            switch (comboboxType)
            {
                case KPIComboboxType.Portfolio:
                    result.Data = data.GroupBy(x => x.Portfolio).Select(x => new KPIComboDataDto(x.Key, megaPortfolios.Contains(x.Key))).OrderBy(x => x.Value).ToList();
                    break;

                case KPIComboboxType.ServiceGroup:
                    result.Data = data.GroupBy(x => x.ServiceGroup).Select(x => new KPIComboDataDto(x.Key, megaServiceGroups.Contains(x.Key))).OrderBy(x => x.Value).ToList();
                    break;

                case KPIComboboxType.Service:
                    result.Data = data.GroupBy(x => x.Service).Select(x => new KPIComboDataDto(x.Key, megaServices.Contains(x.Key))).OrderBy(x => x.Value).ToList();
                    break;

                case KPIComboboxType.KPIMainInterval:
                    result.Data = data.GroupBy(x => x.Interval).Select(x => new KPIComboDataDto(x.Key)).OrderBy(x => x.Value).ToList();
                    break;
            }

            return result;
        }

#warning Below Region should not be here. We have to apply Repository Pattern to the project. We must not access DB at Service Layer(Here). We should access both memory cache & DB at Data Layer (at Repositories). 26-10-21 Akay

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
                _cacheProvider.Set(cachekey, data, TimeSpan.FromHours(this.JiraCacheDurationInHours));
            }

            return data;
        }

        private async Task<List<KpiBackendPortal>> GetKpiBackendPortalsWithCache()
        {
            List<KpiBackendPortal> result = null;

            // Try Cache First...
            var cachekey = $"KpiBackendPortal_Data";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out result);

            if (isFoundAtCache)
                return result;

            // Then Fetch From DB...
            result = await _dbContext.KpiBackendPortals.AsNoTracking().ToListAsync();

            // Update Cache...
            _cacheProvider.Set(cachekey, result, TimeSpan.FromHours(this.JiraCacheDurationInHours));
            return result;
        }

        private async Task<List<PortalOkrBackend>> GetPortalOkrBackendsWithCache()
        {
            List<PortalOkrBackend> result = null;

            // Try Cache First...
            var cachekey = $"PortalOkrBackend_Data";
            var isFoundAtCache = _cacheProvider.TryGetValue(cachekey, out result);

            if (isFoundAtCache)
                return result;

            // Then Fetch From DB...
            result = await _dbContext.PortalOkrBackends.AsNoTracking().ToListAsync();

            // Update Cache...
            _cacheProvider.Set(cachekey, result, TimeSpan.FromHours(this.JiraCacheDurationInHours));
            return result;
        }
        #endregion
    }
}
