using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using Serilog;
using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;

namespace AfinitiPortalAPI.Controllers
{
    public class EmployeeSwitchKnowledgeController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;

        public EmployeeSwitchKnowledgeController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllEmployeeSwitchKnowledge")]
        public async Task<ActionResult<List<EmployeeSwitchKnowledgeGroupedDTO>>> GetEmployeeSwitchKnowledgeLookups()
        {
            Log.Information("GetAllEmployeeSwitchKnowledge: Started");
            List<EmployeeSwitchKnowledgeGroupedDTO> data = new();
            try
            {
                var leftOuterJoinData = await (from paycomEmployee in _context.PaycomEmployeeFulls.AsNoTracking()
                                               join empPlatformKnowledge in _context.RmEmployeeSwitchPlatformKnowledges.AsNoTracking() on paycomEmployee.WorkEmail equals empPlatformKnowledge.EmployeeEmail into empPlatformKnowTemp
                                               from empPlatformKnowledge in empPlatformKnowTemp.DefaultIfEmpty()

                                               join hermesSwitchPlatforms in _context.HermesSwitchPlatforms on empPlatformKnowledge.SwitchPlatformId equals hermesSwitchPlatforms.Id into hermesSwitchPlatformsKnowTemp
                                               from hermesSwitchPlatformsKnow in hermesSwitchPlatformsKnowTemp.DefaultIfEmpty()

                                               join hermesOrg in _context.HermesOrganizations on hermesSwitchPlatformsKnow.ProviderId equals hermesOrg.Id into hermesOrgKnowTemp
                                               from hermesOrg in hermesOrgKnowTemp.DefaultIfEmpty()

                                               where paycomEmployee.ChangeStatus == "CURRENT STATUS" && paycomEmployee.WorkStatus == "WORKING" && paycomEmployee.SubDepartment == "SID"
                                               //&& hermesOrg.Discriminator == "SwitchProvider"

                                               select new EmployeeSwitchKnowledgeDTO()
                                               {
                                                   //Id = empPlatformKnowledge.Id != null ? empPlatformKnowledge.Id : 99,
                                                   EmployeeName = paycomEmployee.FullName,
                                                   EmployeeEmail = paycomEmployee.WorkEmail,
                                                   IsActive = empPlatformKnowledge.IsActive != null ? empPlatformKnowledge.IsActive : false,

                                                   //SwitchProviderId = hermesOrg.Id != null ? hermesOrg.Id : 99,
                                                   //SwitchProviderName = hermesOrg.Name != null ? hermesOrg.Name : "Not Assigned",

                                                   SwitchPlatformId = hermesSwitchPlatformsKnow.Id != null ? hermesSwitchPlatformsKnow.Id : 99,
                                                   SwitchPlatformProviderId = hermesSwitchPlatformsKnow.ProviderId != null ? hermesSwitchPlatformsKnow.ProviderId : 99,
                                                   SwitchPlatformName = hermesSwitchPlatformsKnow.Name != null ? hermesSwitchPlatformsKnow.Name : "Not Assigned",

                                               }).ToListAsync().ConfigureAwait(false);
                data = leftOuterJoinData
                                 .GroupBy(x => new { x.EmployeeEmail, x.EmployeeName })
                                 .Select(y => new EmployeeSwitchKnowledgeGroupedDTO()
                                 {
                                     EmployeeEmail = y.Key.EmployeeEmail,
                                     EmployeeName = y.Key.EmployeeName,
                                     SwitchPlatforms = y.Select(s => new IGenericLookupWithParent()
                                     {
                                         value = (int)(s.SwitchPlatformId != null ? s.SwitchPlatformId : 99),
                                         label = s.SwitchPlatformName != null ? s.SwitchPlatformName : "Not Assigned",
                                         checkStatus = (bool)(s.IsActive != null ? s.IsActive : false),
                                         parent = s.SwitchPlatformProviderId
                                     }
                                     ).ToList()
                                 }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllEmployeeSwitchKnowledge: Error", ex);
            }
            Log.Information("GetAllEmployeeSwitchKnowledge: Completed");
            return Ok(data);
        }

        [HttpPost]
        [Route("AddOrUpdateEmployeeSwitchKnowledge")]
        public async Task<ActionResult<EmployeeSwitchKnowledgeTrimmedDTO>> AddOrUpdateEmployeeSwitchKnowledge([FromBody] EmployeeSwitchKnowledgeTrimmedDTO mappingEntity)
        {
            Log.Information("AddOrUpdateEmployeeSwitchKnowledge:Started");
            try
            {
                var alreadyExistsEntity = _context.RmEmployeeSwitchPlatformKnowledges.FirstOrDefault(f => f.SwitchPlatformId == mappingEntity.SwitchPlatformId && f.EmployeeEmail == mappingEntity.EmployeeEmail);
                if (alreadyExistsEntity != null)// Updated record in db 
                {
                    alreadyExistsEntity.ModifiedAt = DateTime.Now;
                    alreadyExistsEntity.CreatedAt = DateTime.Now;
                    alreadyExistsEntity.IsActive = (bool)mappingEntity.IsActive;

                    mappingEntity.CreatedAt = alreadyExistsEntity.CreatedAt;
                    mappingEntity.ModifiedAt = DateTime.Now;
                    _context.Update(alreadyExistsEntity);
                }
                else// Add new in DB
                {
                    RmEmployeeSwitchPlatformKnowledge newEntity = new();
                    mappingEntity.CreatedAt = DateTime.Now;
                    newEntity = _mapper.Map<EmployeeSwitchKnowledgeTrimmedDTO, RmEmployeeSwitchPlatformKnowledge>(mappingEntity);
                    _context.RmEmployeeSwitchPlatformKnowledges.Add(newEntity);
                }
                await _context.SaveChangesAsync();
                Log.Information("AddOrUpdateEmployeeSwitchKnowledge:Completed");
                return Ok(mappingEntity);
            }
            catch (Exception ex)
            {
                Log.Error("AddOrUpdateEmployeeSwitchKnowledge: Unable to save data: ", ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetSwitchPlatformsKnowledgeHeader")]
        public async Task<IActionResult> GetSwitchPlatformsKnowledgeHeader()
        {
            Log.Information("GetSwitchPlatformsKnowledgeHeader Started");
            List<EmployeeSwitchKnowledgeHeaderGroupedDTO> response = new();
            try
            {
                var data = await (
                     from hermesOrg in _context.HermesOrganizations.AsNoTracking()
                     join hermesSwitchPlatforms in _context.HermesSwitchPlatforms.AsNoTracking() on hermesOrg.Id equals hermesSwitchPlatforms.ProviderId
                     where hermesOrg.Discriminator == "SwitchProvider"
                     select new EmployeeSwitchKnowledgeHeaderDTO()
                     {
                         SwitchProviderId = hermesOrg.Id,
                         SwitchProviderName = hermesOrg.Name,
                         SwitchPlatformId = hermesSwitchPlatforms.Id,
                         SwitchPlatformName = hermesSwitchPlatforms.Name,
                         SwitchPlatformProviderId = hermesSwitchPlatforms.ProviderId
                     }).ToListAsync().ConfigureAwait(false);

                response = data.GroupBy(x => new { x.SwitchProviderId, x.SwitchProviderName })
                                .Select(y => new EmployeeSwitchKnowledgeHeaderGroupedDTO()
                                {
                                    SwitchProviderId = y.Key.SwitchProviderId,
                                    SwitchProviderName = y.Key.SwitchProviderName,
                                    SwitchPlatforms = y.Select(s => new IGenericLookup()
                                    {
                                        value = s.SwitchPlatformId != null ? s.SwitchPlatformId : 99,
                                        label = s.SwitchPlatformName ?? "Not Assigned",
                                        checkStatus = false,
                                    }
                                    ).ToList()
                                }).OrderBy(x => x.SwitchProviderName).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("GetSwitchPlatformsKnowledgeHeader", ex.Message);
            }
            Log.Information("GetSwitchPlatformsKnowledgeHeader Completed");
            return Ok(response);
        }
    }
}
