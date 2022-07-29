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
    public class RmRolesController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public RmRolesController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<ActionResult<List<RmRoleDTO>>> GetAllRoles()
        {
            Log.Information("GetAllRoles:Started");
            List<RmRoleDTO> data = new();
            try
            {
                data = await _context.RmRoles.Where(x => x.IsActive).ProjectTo<RmRoleDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllRoles: Unable to load data: ", ex);
            }
            Log.Information("GetAllRoles:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetRoleById")]
        public async Task<ActionResult<RmRole>> GetRoleById(int id)
        {
            Log.Information("GetRoleById:Started");
            RmRole rmEntity = new();
            try
            {
                rmEntity = await _context.RmRoles.FindAsync(id);
                if (rmEntity == null)
                {
                    Log.Error("GetRoleById: Error: Product is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetRoleById: Unable to fetch data: ", ex);
            }
            Log.Information("GetRoleById:Completed");
            return rmEntity;
        }

        [HttpPatch]
        [Route("UpdateRole")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<RmRoleDTO> patchEntity)
        {
            Log.Information("UpdateRole:Started");
            try
            {
                var entityFromDB = await _context.RmRoles.FindAsync(id); // Get our original person object from the database. 
                RmRoleDTO mappingDTO = _mapper.Map<RmRoleDTO>(entityFromDB); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(mappingDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(mappingDTO, entityFromDB); //Use automapper to map the DTO back ontop of the database object. 
                _context.RmRoles.Update(entityFromDB); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ifEntityExists(id))
                {
                    Log.Error("UpdateRole:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateRole:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateRole:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateRole:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult<RmRole>> CreateRole([FromBody] RmRoleDTO rmRoleDTO)
        {
            Log.Information("CreateRole:Started");
            try
            {
                rmRoleDTO.CreatedAt = DateTime.Now;
                int maxId = _context.RmRoles.Where(u => u.Id != 99).OrderByDescending(u => u.Id).FirstOrDefault().Id;
                rmRoleDTO.Id = maxId + 1;
                RmRole newEntity = new();
                newEntity = _mapper.Map<RmRoleDTO, RmRole>(rmRoleDTO);
                _context.RmRoles.Add(newEntity);
                await _context.SaveChangesAsync();
                Log.Information("CreateRole:Completed");
                return CreatedAtAction("GetRoleById", new { id = newEntity.Id }, rmRoleDTO);
            }
            catch (Exception ex)
            {
                Log.Error("CreateRole: Unable to save data: ", ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("DeleteRoleById")]
        public async Task<IActionResult> DeleteRoleById(int id)
        {
            Log.Information("DeleteRoleById:Started");
            try
            {
                var entityFromDB = await _context.RmRoles.FindAsync(id);
                if (entityFromDB == null)
                {
                    Log.Information("DeleteRoleById:Error: product is null");
                    return NotFound();
                }
                entityFromDB.IsActive = false;
                entityFromDB.ModifiedAt = DateTime.Now;
                _context.RmRoles.Update(entityFromDB);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteRoleById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteRoleById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteRolesInBulk")]
        public async Task<IActionResult> DeleteRolesInBulk(int[] ids)
        {
            Log.Information("DeleteRevDeleteRolesInBulkenueBracketInBulk:Started");
            try
            {
                _context.RmRoles.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifiedAt = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteRolesInBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteRolesInBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }
        private bool ifEntityExists(int id)
        {
            return _context.RmRoles.Any(e => e.Id == id);
        }
    }
}
