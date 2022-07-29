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

namespace AfinitiPortalAPI.Controllers
{
    public class SidRpRegionsController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public SidRpRegionsController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllSidRpRegions")]
        public async Task<ActionResult<List<RmRegionDTO>>> GetSidRpRegions(bool excludeStaticRegions)
        {
            Log.Information("GetAllSidRpRegions:Started");
            List<RmRegionDTO> data = new();
            try
            {
                if (excludeStaticRegions)//exclude 'GLOBAL' and 'Not Mapped to SID Region' regions from list
                {
                    data = await (from regions in _context.RmRegions
                                  join depts in _context.RmDepartments on regions.DepartmentId equals depts.Id into deptsTEMP
                                  from depts in deptsTEMP.DefaultIfEmpty()
                                  join subDepts in _context.RmSubdepartments on regions.SubdepartmentId equals subDepts.Id into subDeptsTEMP
                                  from subDepts in subDeptsTEMP.DefaultIfEmpty()
                                  join teams in _context.RmTeams on regions.TeamId equals teams.Id into teamsTEMP
                                  from teams in teamsTEMP.DefaultIfEmpty()
                                  where regions.IsRegion == true
                                  select new RmRegionDTO()
                                  {
                                      TeamId = teams.Id,
                                      Team = teams.Name,
                                      DepartmentId = depts.Id,
                                      Department = depts.Name,
                                      SubDepartmentId = subDepts.Id,
                                      SubDepartment = subDepts.Name,
                                      RegionalManager = regions.RegionalManager,
                                      Id = regions.Id,
                                      Name = regions.Name,
                                      IsRegion = regions.IsRegion,
                                  }).ToListAsync();
                }
                else
                {
                    data = await (from regions in _context.RmRegions
                                  join depts in _context.RmDepartments on regions.DepartmentId equals depts.Id into deptsTEMP
                                  from depts in deptsTEMP.DefaultIfEmpty()
                                  join subDepts in _context.RmSubdepartments on regions.SubdepartmentId equals subDepts.Id into subDeptsTEMP
                                  from subDepts in subDeptsTEMP.DefaultIfEmpty()
                                  join teams in _context.RmTeams on regions.TeamId equals teams.Id into teamsTEMP
                                  from teams in teamsTEMP.DefaultIfEmpty()
                                  select new RmRegionDTO()
                                  {
                                      TeamId = teams.Id,
                                      Team = teams.Name,
                                      DepartmentId = depts.Id,
                                      Department = depts.Name,
                                      SubDepartmentId = subDepts.Id,
                                      SubDepartment = subDepts.Name,
                                      RegionalManager = regions.RegionalManager,
                                      Id = regions.Id,
                                      Name = regions.Name,
                                      IsRegion = regions.IsRegion,
                                  }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetAllSidRpRegions: Unable to load data: ", ex);
            }
            Log.Information("GetAllSidRpRegions:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetSidRpRegionById")]
        public async Task<ActionResult<RmRegion>> GetSidRpRegion(int id)
        {
            Log.Information("GetSidRpRegionById:Started");
            RmRegion sidRpRegion = new();
            try
            {
                sidRpRegion = await _context.RmRegions.FindAsync(id);
                if (sidRpRegion == null)
                {
                    Log.Error("GetSidRpRegionById: Error: sidRpRegion is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetSidRpRegionById: Unable to fetch data: ", ex);
            }
            Log.Information("GetSidRpRegionById:Completed");
            return sidRpRegion;
        }

        [HttpPatch]
        [Route("UpdateSidRpRegion")]
        public async Task<IActionResult> PatchSidRpRegion(int id, [FromBody] JsonPatchDocument<RmRegionDTO> patchEntity)
        {
            Log.Information("UpdateSidRpRegion:Started");
            try
            {
                var sidRpRegionEntity = await _context.RmRegions.FindAsync(id); // Get our original person object from the database. 
                RmRegionDTO personDTO = _mapper.Map<RmRegionDTO>(sidRpRegionEntity); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(personDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(personDTO, sidRpRegionEntity); //Use automapper to map the DTO back ontop of the database object. 
                _context.RmRegions.Update(sidRpRegionEntity); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SidRpRegionExists(id))
                {
                    Log.Error("UpdateSidRpRegion:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateSidRpRegion:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateSidRpRegion:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateSidRpRegion:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateSidRpRegion")]
        public async Task<ActionResult<RmRegion>> PostSidRpRegion([FromBody] RmRegionDTO sidRpRegionDTO)
        {
            Log.Information("CreateSidRpRegion:Started");
            try
            {
                int departId = _context.RmDepartments.FirstOrDefault(x => x.Name == "GSD").Id;
                int subDept = _context.RmSubdepartments.FirstOrDefault(x => x.DepartmentId == departId && x.Name == "SID").Id;
                sidRpRegionDTO.DepartmentId = departId;
                sidRpRegionDTO.SubDepartmentId = subDept;
                sidRpRegionDTO.CreatedAt = DateTime.Now;
                RmRegion rmRegion = new();
                rmRegion = _mapper.Map<RmRegionDTO, RmRegion>(sidRpRegionDTO);
                _context.RmRegions.Add(rmRegion);
                await _context.SaveChangesAsync();
                sidRpRegionDTO.Id = rmRegion.Id;
                Log.Information("CreateSidRpRegion:Completed");
                return CreatedAtAction("GetSidRpRegion", new { id = rmRegion.Id }, sidRpRegionDTO);
            }
            catch (Exception ex)
            {
                Log.Error("CreateSidRpRegion: Unable to save data: ", ex);
            }
            Log.Information("CreateSidRpRegion:Completed");
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteSidRpRegionById")]
        public async Task<IActionResult> DeleteSidRpRegion(int id)
        {
            Log.Information("DeleteSidRpRegionById:Started");
            try
            {
                var sidRpRegion = await _context.RmRegions.FindAsync(id);
                if (sidRpRegion == null)
                {
                    Log.Information("DeleteSidRpRegionById:Error: sidRpRegion is null");
                    return NotFound();
                }
                sidRpRegion.IsActive = false;
                sidRpRegion.ModifiedAt = DateTime.Now;
                _context.RmRegions.Update(sidRpRegion);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteSidRpRegionById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteSidRpRegionById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteSidRpRegionInBulk")]
        public async Task<IActionResult> DeleteSidRpRegionBulk(int[] ids)
        {
            Log.Information("DeleteSidRpRegionInBulk:Started");
            try
            {
                _context.RmRegions.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifiedAt = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteSidRpRegionInBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteSidRpRegionInBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        private bool SidRpRegionExists(int id)
        {
            return _context.RmRegions.Any(e => e.Id == id);
        }
    }
}
