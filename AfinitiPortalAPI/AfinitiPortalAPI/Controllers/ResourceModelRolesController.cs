using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using Serilog;
using Microsoft.AspNetCore.JsonPatch;
using AfinitiPortalAPI.Shared.DTOs;

namespace AfinitiPortalAPI.Controllers
{
    public class ResourceModelRolesController : BaseController
    {
        private readonly PortalDBContext _context;

        public ResourceModelRolesController(PortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllResourceModelRoles")]
        public async Task<ActionResult<List<ResourceModelRolesLookupDTO>>> GetResourceModelRoles()
        {
            Log.Information("GetAllResourceModelRoles:Started");
            List<ResourceModelRolesLookupDTO> data = new();
            try
            {
                data = await _context.ResourceModelRolesLookups.Join(_context.ResourceModelRolesGroups,
                    lookup => lookup.RoleGroupId,
                    groups => groups.RoleGroupId,
                    (lookup, groups) => new ResourceModelRolesLookupDTO()
                    {
                        Id = lookup.Id,
                        RoleResourceModel = lookup.RoleResourceModel,
                        RoleGroupId = lookup.RoleGroupId,
                        RoleGroupName = groups.RoleGroupName,
                        PaycomSubDepartment = lookup.PaycomSubDepartment,
                        Status = lookup.Status,
                        IsDedicated = lookup.IsDedicated,
                        Shifts = lookup.Shifts,
                        InsertionDate = lookup.InsertionDate,
                        ModifyDate = lookup.ModifyDate,
                        IsActive = lookup.IsActive,
                    }
                    ).Where(x => x.IsActive == true).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllResourceModelRoles: Unable to load data: ", ex);
            }
            Log.Information("GetAllResourceModelRoles:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetResourceModelRoleById")]//not in use so far
        public async Task<ActionResult<ResourceModelRolesLookup>> GetResourceModelRole(int id)
        {
            Log.Information("GetResourceModelRoleById:Started");
            ResourceModelRolesLookup resourceModelRolesLookup = new();
            try
            {
                resourceModelRolesLookup = await _context.ResourceModelRolesLookups.FindAsync(id);
                if (resourceModelRolesLookup == null)
                {
                    Log.Error("GetResourceModelRoleById: Error: ResourceModelRole is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetResourceModelRoleById: Unable to fetch data: ", ex);
            }
            Log.Information("GetResourceModelRoleById:Completed");
            return resourceModelRolesLookup;
        }

        [HttpPatch]
        [Route("UpdateResourceModelRole")]
        public async Task<ActionResult> PatchResourceModelRole(int id, [FromBody] JsonPatchDocument<ResourceModelRolesLookup> patchEntity)
        {
            Log.Information("UpdateResourceModelRole:Started");
            ResourceModelRolesGroup resourceModelRolesGroup = new();
            try
            {
                if (patchEntity.Operations[0].path == "roleGroupId" && patchEntity.Operations[1].path == "roleGroupName")
                {
                    resourceModelRolesGroup = createResourceModelGroupIfNotExists(patchEntity.Operations[1].value.ToString());
                    if (Convert.ToInt32(patchEntity.Operations[0].value) != resourceModelRolesGroup.RoleGroupId)
                    {
                        patchEntity.Operations[0].value = resourceModelRolesGroup.RoleGroupId;
                    }
                    patchEntity.Operations.Remove(patchEntity.Operations[1]);
                }
                var ResourceModelRolesLookupEntity = await _context.ResourceModelRolesLookups.FindAsync(id);
                if (ResourceModelRolesLookupEntity == null)
                    return NotFound("No Region found in system. Please contact BI");
                patchEntity.ApplyTo(ResourceModelRolesLookupEntity, ModelState); // Must have Microsoft.AspNetCore.Mvc.NewtonsoftJson installed  
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Update(ResourceModelRolesLookupEntity);
                await _context.SaveChangesAsync();
                return CreatedAtAction("PatchResourceModelRole", new { Id = resourceModelRolesGroup.RoleGroupId });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ResourceModelRoleExists(id))
                {
                    Log.Error("UpdateResourceModelRole:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateResourceModelRole:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateResourceModelRole:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateResourceModelRole:Completed");
            return Ok();
        }

        private ResourceModelRolesGroup createResourceModelGroupIfNotExists(string resourceModelRoleName)
        {
            var alreadyExistsRoleGroup = _context.ResourceModelRolesGroups.Where(f => f.IsActive == true && f.RoleGroupName == resourceModelRoleName);
            ResourceModelRolesGroup resourceModelRolesGroup = new();
            int roleGroupId = 0;
            if (!alreadyExistsRoleGroup.Any())
            {
                if (_context.ResourceModelRolesGroups.Where(f => f.IsActive == true).Any())
                    roleGroupId = _context.ResourceModelRolesGroups.Where(f => f.IsActive == true).Max(p => p.RoleGroupId) + 1;
                else roleGroupId = 1;
                resourceModelRolesGroup.IsActive = true;
                resourceModelRolesGroup.RoleGroupId = roleGroupId;
                resourceModelRolesGroup.RoleGroupName = resourceModelRoleName;
                _context.ResourceModelRolesGroups.Add(resourceModelRolesGroup);
                return resourceModelRolesGroup;
            }
            else
                return alreadyExistsRoleGroup.FirstOrDefault();
        }

        [HttpPost]
        [Route("CreateResourceModelRole")]
        public async Task<ActionResult<ResourceModelRolesLookup>> CreateResourceModelRole([FromBody] ResourceModelRolesLookupDTO resourceModelRolesLookupDTO)
        {
            Log.Information("CreateResourceModelRole:Started");
            try
            {
                ResourceModelRolesGroup resourceModelRolesGroup = createResourceModelGroupIfNotExists(resourceModelRolesLookupDTO.RoleGroupName);
                ResourceModelRolesLookup resourceModelRolesLookup = new()
                {
                    RoleGroupId = resourceModelRolesGroup.RoleGroupId != 0 ? resourceModelRolesGroup.RoleGroupId : resourceModelRolesLookupDTO.RoleGroupId,
                    IsActive = resourceModelRolesLookupDTO.IsActive,
                    Shifts = resourceModelRolesLookupDTO.Shifts,
                    Status = resourceModelRolesLookupDTO.Status,
                    PaycomSubDepartment = resourceModelRolesLookupDTO.PaycomSubDepartment,
                    IsDedicated = resourceModelRolesLookupDTO.IsDedicated,
                    InsertionDate = DateTime.Now,
                    RoleResourceModel = resourceModelRolesLookupDTO.RoleResourceModel,
                };
                _context.ResourceModelRolesLookups.Add(resourceModelRolesLookup);
                await _context.SaveChangesAsync();
                resourceModelRolesLookupDTO.RoleGroupId = resourceModelRolesLookup.RoleGroupId;
                resourceModelRolesLookupDTO.Id = resourceModelRolesLookup.Id;
                Log.Information("CreateResourceModelRole:Completed");
                return CreatedAtAction("CreateResourceModelRole", new { id = resourceModelRolesLookup.Id }, resourceModelRolesLookupDTO);
            }
            catch (Exception ex)
            {
                Log.Error("CreateResourceModelRole: Unable to save data: ", ex);
            }
            Log.Information("CreateResourceModelRole:Completed");
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteResourceModelRoleById")]
        public async Task<IActionResult> DeleteResourceModelRole(int id)
        {
            Log.Information("DeleteResourceModelRoleById:Started");
            try
            {
                var resourceModelRolesLookup = await _context.ResourceModelRolesLookups.FindAsync(id);
                if (resourceModelRolesLookup == null)
                {
                    Log.Information("DeleteResourceModelRoleById:Error: ResourceModelRole is null");
                    return NotFound();
                }
                resourceModelRolesLookup.IsActive = false;
                resourceModelRolesLookup.ModifyDate = DateTime.Now;
                _context.ResourceModelRolesLookups.Update(resourceModelRolesLookup);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteResourceModelRoleById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteResourceModelRoleById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteResourceModelRolesInBulk")]
        public async Task<IActionResult> DeleteResourceModelRolesBulk(int[] ids)
        {
            Log.Information("DeleteResourceModelRolesInBulk:Started");
            try
            {
                _context.ResourceModelRolesLookups.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifyDate = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteResourceModelRolesInBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteResourceModelRolesInBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        private bool ResourceModelRoleExists(int id)
        {
            return _context.ResourceModelRolesLookups.Any(e => e.Id == id);
        }
    }
}
