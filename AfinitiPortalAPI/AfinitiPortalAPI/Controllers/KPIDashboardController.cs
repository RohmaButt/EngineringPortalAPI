using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.DTOs;
using AfinitiPortalAPI.Shared.Enums;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Controllers
{
    public class KPIDashboardController : BaseController
    {
        private readonly IKPIDashboardService _kpiDashboardService;

        public KPIDashboardController(IKPIDashboardService kpiDashboardService)
        {
            _kpiDashboardService = kpiDashboardService;
        }

        [HttpGet("getChartData/quality")]
        public async Task<IActionResult> GetQualityData([FromQuery] GetChartDataRequest request)
        {
            var response = await _kpiDashboardService.GetQualityChartData(request, false);
            return Ok(response);
        }

        [HttpGet("getChartData/deliverySpeed")]
        public async Task<IActionResult> GetDeliverySpeedData([FromQuery] GetChartDataRequest request)
        {
            var response = await _kpiDashboardService.GetDeliverySpeedChartData(request, false);
            return Ok(response);
        }

        [HttpGet("getChartData/productionQuality")]
        public async Task<IActionResult> GetProductionQualityData([FromQuery] GetChartDataRequest request)
        {
            var response = await _kpiDashboardService.GetProductionQualityChartData(request, false);
            return Ok(response);
        }

        [HttpGet("getChartData/bulk")]
        public async Task<IActionResult> GetBulkChartData([FromQuery] GetBulkChartDataRequest request)
        {
            var response = await _kpiDashboardService.GetBulkChartData(request);
            return Ok(response);
        }

        [HttpGet("getPortfolios")]
        public async Task<IActionResult> GetPortfolios()
        {
            var response = await _kpiDashboardService.GetComboDataForKPI(KPIComboboxType.Portfolio);
            return Ok(response);
        }

        [HttpGet("getServiceGroups")]
        public async Task<IActionResult> GetServiceGroups()
        {
            var response = await _kpiDashboardService.GetComboDataForKPI(KPIComboboxType.ServiceGroup);
            return Ok(response);
        }

        [HttpGet("getServices")]
        public async Task<IActionResult> GetServices()
        {
            var response = await _kpiDashboardService.GetComboDataForKPI(KPIComboboxType.Service);
            return Ok(response);
        }

        [HttpGet("getKpiFilterData")]
        public async Task<IActionResult> GetKPIFilterData([FromQuery] KPIComboboxType dataType)
        {
            var response = await _kpiDashboardService.GetComboDataForKPI(dataType);
            return Ok(response);
        }

        [HttpGet("getCatalogueInfo")]
        public async Task<IActionResult> GetCatalogueInfo([FromQuery] string email)
        {
            var response = await _kpiDashboardService.GetEmployeeServiceCatalogueInfo(email);
            return Ok(response);
        }

        [HttpGet("getOkrData")]
        public async Task<IActionResult> GetOKRData([FromQuery] string interval, [FromQuery] string portfolio = null, [FromQuery] string serviceGroup = null, [FromQuery] string service = null, [FromQuery] string email = null)
        {
            var response = await _kpiDashboardService.GetOKRTableData(portfolio, serviceGroup, service, interval, email);
            return Ok(response);
        }

        [HttpGet("getOkrIntervals")]
        public async Task<IActionResult> GetOKRIntervals()
        {
            var response = await _kpiDashboardService.GetOKRIntervals();
            return Ok(response);
        }

        [HttpGet("getOkrFilterData")]
        public async Task<IActionResult> GetOkrFilterData([FromQuery] string email = null)
        {
            var response = await _kpiDashboardService.GetComboDataForOKR(email);
            return Ok(response);
        }

        [HttpGet("getKpiIntervals")]
        public async Task<IActionResult> GetKPIIntervals()
        {
            var response = await _kpiDashboardService.GetComboDataForKPI(KPIComboboxType.KPIMainInterval);
            return Ok(response);
        }
    }
}
