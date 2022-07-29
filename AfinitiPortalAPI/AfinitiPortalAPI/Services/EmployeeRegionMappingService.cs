using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.services
{
    public interface IEmployeeRegionMappingService
    {
        Task<RmEmployeeRegionalMappingDTO> AddOrUpdateEmployeeRegionMapping(RmEmployeeRegionalMappingDTO accountRegionMapping);
        Task<dynamic> GetEmployeeRegionMapping();
    }
    public class EmployeeRegionMappingService : IEmployeeRegionMappingService
    {
        private readonly PortalDBContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeRegionMappingService(PortalDBContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<dynamic> GetEmployeeRegionMapping()
        {
            Log.Information("GetEmployeeRegionMapping Service:Started");
            var leftOuterJoinData =
                     from paycomEmployees in _dbContext.PaycomEmployeeFulls.AsNoTracking()
                     join employeeRegionMapping in _dbContext.RmEmployeeRegionalMappings.AsNoTracking() on paycomEmployees.WorkEmail equals employeeRegionMapping.EmployeeEmail into tempEmployeeRegionMapping
                     from employeeRegionMapping in tempEmployeeRegionMapping.DefaultIfEmpty()
                     join regions in _dbContext.RmRegions.AsNoTracking() on employeeRegionMapping.RegionId equals regions.Id into TempRegions
                     from regions in TempRegions.DefaultIfEmpty()
                     where paycomEmployees.ChangeStatus == "CURRENT STATUS" && paycomEmployees.WorkStatus == "WORKING" && paycomEmployees.SubDepartment == "SID" //&& employeeRegionMapping.IsActive
                     select new
                     {
                         EmployeeEmail = paycomEmployees.WorkEmail,
                         Name = paycomEmployees.FullName,
                         TeamName = paycomEmployees.Team,
                         paycomEmployees.Country,
                         paycomEmployees.LegalCountry,
                         RegionId = employeeRegionMapping.Id != null ? regions.Id : 98,
                         RegionName = employeeRegionMapping.Id != null ? regions.Name : "Not Mapped to SID Region"
                     };
            Log.Information("GetEmployeeRegionMapping Service:Completed");
            return await leftOuterJoinData.ToListAsync().ConfigureAwait(false);
        }

        public async Task<RmEmployeeRegionalMappingDTO> AddOrUpdateEmployeeRegionMapping(RmEmployeeRegionalMappingDTO accountRegionMapping)
        {
            Log.Information("AddOrUpdateEmployeeRegionMapping Service:Started");
            var alreadyExistsEntity = _dbContext.RmEmployeeRegionalMappings.FirstOrDefault(f => f.IsActive == true && f.EmployeeEmail == accountRegionMapping.EmployeeEmail);
            if (alreadyExistsEntity != null)// Updated record in db 
            {
                alreadyExistsEntity.RegionId = accountRegionMapping.RegionId;
                alreadyExistsEntity.ModifiedAt = System.DateTime.Now;
                _dbContext.Update(alreadyExistsEntity);
            }
            else// Add new in DB
            {
                RmEmployeeRegionalMapping newEntity = new();
                accountRegionMapping.CreatedAt = System.DateTime.Now;
                accountRegionMapping.IsActive = true;
                newEntity = _mapper.Map<RmEmployeeRegionalMappingDTO, RmEmployeeRegionalMapping>(accountRegionMapping);
                _dbContext.RmEmployeeRegionalMappings.Add(newEntity);
            }
            await _dbContext.SaveChangesAsync();
            Log.Information("AddOrUpdateEmployeeRegionMapping Service:Completed");
            return accountRegionMapping;
        }

    }
}
