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

namespace AfinitiPortalAPI.Controllers
{
    public class SidRpWorkflow1Controller : BaseController
    {
        private readonly PortalDBContext _context;
        public SidRpWorkflow1Controller(PortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllSidRpWorkflow1")]
        public async Task<ActionResult<List<SidRpWorkflow1>>> GetSidRpWorkflow1s()
        {
            Log.Information("GetAllSidRpWorkflow1:Started");
            List<SidRpWorkflow1> data = new();
            try
            {
                data = await _context.SidRpWorkflow1s.Where(z => z.IsActive == true).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllSidRpWorkflow1: Unable to load data: ", ex);
            }
            Log.Information("GetAllSidRpWorkflow1:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetSidRpWorkflow1ById")]
        public async Task<ActionResult<SidRpWorkflow1>> GetSidRpWorkflow1(int id)
        {
            Log.Information("GetSidRpWorkflow1ById:Started");
            SidRpWorkflow1 SidRpWorkflow1 = new();
            try
            {
                SidRpWorkflow1 = await _context.SidRpWorkflow1s.FindAsync(id);
                if (SidRpWorkflow1 == null)
                {
                    Log.Error("GetSidRpWorkflow1ById: Error: SidRpWorkflow1 is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetSidRpWorkflow1ById: Unable to fetch data: ", ex);
            }
            Log.Information("GetSidRpWorkflow1ById:Completed");
            return SidRpWorkflow1;
        }

        [HttpPatch]
        [Route("UpdateSidRpWorkflow1")]
        public async Task<IActionResult> PatchSidRpWorkflow1(int id, [FromBody] JsonPatchDocument<SidRpWorkflow1> patchEntity)
        {
            Log.Information("UpdateSidRpWorkflow1:Started");
            try
            {
                var sidRpWorkFlowEntity = await _context.SidRpWorkflow1s.FindAsync(id);
                if (sidRpWorkFlowEntity == null)
                    return NotFound("No work flow found in system. Please contact BI");
                patchEntity.ApplyTo(sidRpWorkFlowEntity, ModelState); // Must have Microsoft.AspNetCore.Mvc.NewtonsoftJson installed  
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Update(sidRpWorkFlowEntity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SidRpWorkflow1Exists(id))
                {
                    Log.Error("UpdateSidRpWorkflow1:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateSidRpWorkflow1:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateSidRpWorkflow1:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateSidRpWorkflow1:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateSidRpWorkflow1")]
        public async Task<ActionResult<SidRpWorkflow1>> PostSidRpWorkflow1([FromBody] SidRpWorkflow1 SidRpWorkflow1)
        {
            Log.Information("CreateSidRpWorkflow1:Started");
            try
            {
                _context.SidRpWorkflow1s.Add(SidRpWorkflow1);
                await _context.SaveChangesAsync();
                Log.Information("CreateSidRpWorkflow1:Completed");
                return CreatedAtAction("GetSidRpWorkflow1", new { id = SidRpWorkflow1.Id }, SidRpWorkflow1);
            }
            catch (Exception ex)
            {
                Log.Error("CreateSidRpWorkflow1: Unable to save data: ", ex);
            }
            Log.Information("CreateSidRpWorkflow1:Completed");
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteSidRpWorkflow1ById")]
        public async Task<IActionResult> DeleteSidRpWorkflow1(int id)
        {
            Log.Information("DeleteSidRpWorkflow1ById:Started");
            try
            {
                var SidRpWorkflow1 = await _context.SidRpWorkflow1s.FindAsync(id);
                if (SidRpWorkflow1 == null)
                {
                    Log.Information("DeleteSidRpWorkflow1ById:Error: SidRpWorkflow1 is null");
                    return NotFound();
                }
                SidRpWorkflow1.IsActive = false;
                SidRpWorkflow1.ModifyDate = DateTime.Now;
                _context.SidRpWorkflow1s.Update(SidRpWorkflow1);//Soft delete record
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteSidRpWorkflow1ById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteSidRpWorkflow1ById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteSidRpWorkflow1InBulk")]
        public async Task<IActionResult> DeleteSidRpWorkflow1Bulk(int[] ids)
        {
            Log.Information("DeleteSidRpWorkflow1InBulk:Started");
            try
            {
                _context.SidRpWorkflow1s.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifyDate = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteSidRpWorkflow1InBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteSidRpWorkflow1InBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        private bool SidRpWorkflow1Exists(int id)
        {
            return _context.SidRpWorkflow1s.Any(e => e.Id == id);
        }
    }
}
