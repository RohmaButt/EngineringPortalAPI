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
    public interface IAcctRegionMappingService
    {
        Task<RmAccountRegionalMappingDTO> AddOrUpdateAccountRegionMapping(RmAccountRegionalMappingDTO accountRegionMapping);
        Task<dynamic> GetAccountRegionMapping();
    }
    public class AcctRegionMappingService : IAcctRegionMappingService
    {
        private readonly PortalDBContext _dbContext;
        private readonly IMapper _mapper;

        public AcctRegionMappingService(PortalDBContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<dynamic> GetAccountRegionMapping()
        {
            Log.Information("GetAccountRegionMapping Service:Started");
            var leftOuterJoinData =
                     from hermesOrg in _dbContext.HermesOrganizations.AsNoTracking()
                     join hermesCountries in _dbContext.HermesCountries.AsNoTracking() on hermesOrg.CountryId equals hermesCountries.Id into tempHermesCountries
                     from hermesCountries in tempHermesCountries.DefaultIfEmpty()
                     join accountRegionMapping in _dbContext.RmAccountRegionalMappings.AsNoTracking() on hermesOrg.Id equals accountRegionMapping.AccountId into tempAccountRegionMapping
                     from accountRegionMapping in tempAccountRegionMapping.DefaultIfEmpty()
                     join regions in _dbContext.RmRegions.AsNoTracking() on accountRegionMapping.RegionId equals regions.Id into tempRegions
                     from regions in tempRegions.DefaultIfEmpty()
                     where hermesOrg.Discriminator == "Account" && hermesOrg.Deleted == 0
                     select new
                     {
                         AccountId = hermesOrg.Id,
                         AccountName = hermesOrg.Name,
                         AccountStatus = hermesOrg.Inactive,
                         CountryId = hermesCountries.Id,
                         AccountCountry = hermesCountries.Name,
                         SidRegionid = accountRegionMapping.RegionId != null ? accountRegionMapping.RegionId : 98,
                         SidRegionName = accountRegionMapping.RegionId != null ? regions.Name : "Not Mapped to SID Region"
                     };
            Log.Information("GetAccountRegionMapping Service:Completed");
            return await leftOuterJoinData.ToListAsync().ConfigureAwait(false); // <-- notice the `await` here. And always use `ConfigureAwait`.
        }

        public async Task<RmAccountRegionalMappingDTO> AddOrUpdateAccountRegionMapping(RmAccountRegionalMappingDTO accountRegionMapping)
        {
            Log.Information("AddOrUpdateAccountRegionMapping Service:Started");
            var alreadyExistsEntity = _dbContext.RmAccountRegionalMappings.FirstOrDefault(f => f.IsActive == true && f.AccountId == accountRegionMapping.AccountId);
            if (alreadyExistsEntity != null)// Updated record in db 
            {
                alreadyExistsEntity.RegionId = accountRegionMapping.RegionId;
                alreadyExistsEntity.ModifiedAt = System.DateTime.Now;
                _dbContext.Update(alreadyExistsEntity);
            }
            else// Add new in DB
            {
                RmAccountRegionalMapping accountRegionNew = new();
                accountRegionMapping.CreatedAt = System.DateTime.Now;
                accountRegionMapping.IsActive = true;
                accountRegionNew = _mapper.Map<RmAccountRegionalMappingDTO, RmAccountRegionalMapping>(accountRegionMapping);
                _dbContext.RmAccountRegionalMappings.Add(accountRegionNew);
            }
            await _dbContext.SaveChangesAsync();
            Log.Information("AddOrUpdateAccountRegionMapping Service:Completed");
            return accountRegionMapping;
        }

    }
}
