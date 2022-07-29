using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using Serilog;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper.QueryableExtensions;

namespace AfinitiPortalAPI.Controllers
{
    public class TsWorklogsController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public TsWorklogsController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllWorklogsForAdmin")]
        public async Task<ActionResult<List<TsWorklogAdminDTO>>> GetAllWorklogsForAdmin(TsWorklogDTO entityDTO)
        {
            Log.Information("GetAllWorklogsForAdmin:Started");
            List<TsWorklogAdminDTO> data = new();
            try
            {
                //1. if Hermes Accounts against any Jira Category
                var hermesAccountsData = await (
               from worklogs in _context.TsWorklogs.AsNoTracking()
               join jiraIssueCats in _context.TsJiraIssueCategories.AsNoTracking() on worklogs.CategoryId equals jiraIssueCats.Id
               join hermesOrg in _context.HermesOrganizations.AsNoTracking() on worklogs.TypeId equals hermesOrg.Id
               where jiraIssueCats.HermesAccount == true && hermesOrg.Deleted == 0 && hermesOrg.Discriminator == "Account"
               select new TsWorklogAdminDTO()
               {
                   EmployeeEmail = worklogs.EmployeeEmail,
                   TypeName = hermesOrg.Name,
                   CategoryName = jiraIssueCats.Name,
                   Week1 = worklogs.Week == "1" ? worklogs.LoggedHoursPercent : 0,
                   Week2 = worklogs.Week == "2" ? worklogs.LoggedHoursPercent : 0,
                   Week3 = worklogs.Week == "3" ? worklogs.LoggedHoursPercent : 0,
                   Week4 = worklogs.Week == "4" ? worklogs.LoggedHoursPercent : 0,
                   Week5 = worklogs.Week == "5" ? worklogs.LoggedHoursPercent : 0
               }).ToListAsync().ConfigureAwait(false);

                //2. if Jira Issue Types against any Jira Category
                var jiraIssueData = await (
                 from worklogs in _context.TsWorklogs.AsNoTracking()
                 join jiraIssueCats in _context.TsJiraIssueCategories.AsNoTracking() on worklogs.CategoryId equals jiraIssueCats.Id
                 join jiraIssueTypes in _context.TsJiraIssueTypes.AsNoTracking() on worklogs.TypeId equals jiraIssueTypes.Id
                 where jiraIssueCats.HermesAccount == false
                 select new TsWorklogAdminDTO()
                 {
                     EmployeeEmail = worklogs.EmployeeEmail,
                     TypeName = jiraIssueTypes.Name,
                     CategoryName = jiraIssueCats.Name,
                     Week1 = worklogs.Week == "1" ? worklogs.LoggedHoursPercent : 0,
                     Week2 = worklogs.Week == "2" ? worklogs.LoggedHoursPercent : 0,
                     Week3 = worklogs.Week == "3" ? worklogs.LoggedHoursPercent : 0,
                     Week4 = worklogs.Week == "4" ? worklogs.LoggedHoursPercent : 0,
                     Week5 = worklogs.Week == "5" ? worklogs.LoggedHoursPercent : 0
                 }).ToListAsync().ConfigureAwait(false);

                data.AddRange(jiraIssueData);
                data.AddRange(hermesAccountsData);
            }
            catch (Exception ex)
            {
                Log.Error("GetAllWorklogsForAdmin: Unable to load data: ", ex);
            }
            Log.Information("GetAllWorklogsForAdmin:Completed");
            return Ok(data.OrderBy(o => o.EmployeeEmail));
        }

        [HttpGet]
        [Route("GetWorklogsForDateRange")]
        public async Task<ActionResult<List<TsWorklogTrimmedDTO>>> GetWorklogsForDateRange(TsWorklogDTO entityDTO)
        {
            Log.Information("GetAllWorklogsForDateRange:Started");
            List<TsWorklogTrimmedDTO> data = new();
            try
            {
                data = await _context.TsWorklogs.AsNoTracking().Where(x => x.IsActive && x.StartDate >= entityDTO.StartDate && x.EndDate <= entityDTO.EndDate
                && x.EmployeeEmail == entityDTO.EmployeeEmail
                ).ProjectTo<TsWorklogTrimmedDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllWorklogsForDateRange: Unable to load data: ", ex);
            }
            Log.Information("GetAllWorklogsForDateRange:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetAllWorklogs")]
        public async Task<ActionResult<List<TsWorklogDTO>>> GetAllWorklogs()
        {
            Log.Information("GetAllWorklogs:Started");
            List<TsWorklogDTO> data = new();
            try
            {
                data = await _context.TsWorklogs.AsNoTracking().Where(x => x.IsActive).ProjectTo<TsWorklogDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllWorklogs: Unable to load data: ", ex);
            }
            Log.Information("GetAllWorklogs:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetWorklogById")]
        public async Task<ActionResult<TsWorklog>> GetWorklogById(int id)
        {
            Log.Information("GetWorklogById:Started");
            TsWorklog rmEntity = new();
            try
            {
                rmEntity = await _context.TsWorklogs.FindAsync(id);
                if (rmEntity == null)
                {
                    Log.Error("GetWorklogById: Error: Product is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetWorklogById: Unable to fetch data: ", ex);
            }
            Log.Information("GetWorklogById:Completed");
            return rmEntity;
        }

        [HttpPatch]
        [Route("UpdateWorklog")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TsWorklogDTO> patchEntity)
        {
            Log.Information("UpdateWorklog:Started");
            try
            {
                var entityFromDB = await _context.TsWorklogs.FindAsync(id); // Get our original person object from the database. 
                TsWorklogDTO mappingDTO = _mapper.Map<TsWorklogDTO>(entityFromDB); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(mappingDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(mappingDTO, entityFromDB); //Use automapper to map the DTO back ontop of the database object. 
                _context.TsWorklogs.Update(entityFromDB); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ifEntityExists(id))
                {
                    Log.Error("UpdateWorklog:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateWorklog:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateWorklog:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateWorklog:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateWorklog")]
        public async Task<ActionResult<TsWorklog>> CreateWorklog([FromBody] TsWorklogDTO entityDTO)
        {
            Log.Information("CreateWorklog:Started");
            try
            {
                entityDTO.InsertionDate = DateTime.Now;
                TsWorklog newEntity = new();
                newEntity = _mapper.Map<TsWorklogDTO, TsWorklog>(entityDTO);
                _context.TsWorklogs.Add(newEntity);
                await _context.SaveChangesAsync();
                Log.Information("CreateWorklog:Completed");
                return CreatedAtAction("GetWorklogById", newEntity);
            }
            catch (Exception ex)
            {
                Log.Error("CreateWorklog: Unable to save data: ", ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("DeleteWorklogById")]
        public async Task<IActionResult> DeleteWorklogById(int id)
        {
            Log.Information("DeleteWorklogById:Started");
            try
            {
                var entityFromDB = await _context.TsWorklogs.FindAsync(id);
                if (entityFromDB == null)
                {
                    Log.Information("DeleteWorklogById:Error: product is null");
                    return NotFound();
                }
                entityFromDB.IsActive = false;
                entityFromDB.ModifyDate = DateTime.Now;
                _context.TsWorklogs.Update(entityFromDB);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteWorklogById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteWorklogById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }
        private bool ifEntityExists(int id)
        {
            return _context.TsWorklogs.Any(e => e.Id == id);
        }
    }
}
