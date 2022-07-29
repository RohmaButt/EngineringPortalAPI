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
    public class TsPeriodMgmtController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public TsPeriodMgmtController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllPeriods")]
        public async Task<ActionResult<List<TsPeriodDTO>>> GetAll()
        {
            Log.Information("GetAll:Started");
            List<TsPeriodDTO> data = new();
            try
            {
                data = await _context.TsPeriods.Where(x => x.IsActive).ProjectTo<TsPeriodDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllPeriods: Unable to load data: ", ex);
            }
            Log.Information("GetAllPeriods:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetPeriodById")]
        public async Task<ActionResult<TsPeriod>> GetPeriodById(int id)
        {
            Log.Information("GetPeriodById:Started");
            TsPeriod rmEntity = new();
            try
            {
                rmEntity = await _context.TsPeriods.FindAsync(id);
                if (rmEntity == null)
                {
                    Log.Error("GetPeriodById: Error: Product is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetPeriodById: Unable to fetch data: ", ex);
            }
            Log.Information("GetPeriodById:Completed");
            return rmEntity;
        }

        [HttpPatch]
        [Route("UpdatePeriod")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TsPeriodDTO> patchEntity)
        {
            Log.Information("UpdatePeriod:Started");
            try
            {
                var entityFromDB = await _context.TsPeriods.FindAsync(id); // Get our original person object from the database. 
                TsPeriodDTO mappingDTO = _mapper.Map<TsPeriodDTO>(entityFromDB); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(mappingDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(mappingDTO, entityFromDB); //Use automapper to map the DTO back ontop of the database object. 
                _context.TsPeriods.Update(entityFromDB); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ifEntityExists(id))
                {
                    Log.Error("UpdatePeriod:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdatePeriod:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdatePeriod:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdatePeriod:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreatePeriod")]
        public async Task<ActionResult<TsPeriod>> CreatePeriod([FromBody] TsPeriodDTO entityDTO)
        {
            Log.Information("CreatePeriod:Started");
            try
            {
                entityDTO.InsertionDate = DateTime.Now;
                TsPeriod newEntity = new();
                newEntity = _mapper.Map<TsPeriodDTO, TsPeriod>(entityDTO);
                _context.TsPeriods.Add(newEntity);
                await _context.SaveChangesAsync();
                Log.Information("CreatePeriod:Completed");
                return CreatedAtAction("GetPeriodById", newEntity);
            }
            catch (Exception ex)
            {
                Log.Error("CreatePeriod: Unable to save data: ", ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("DeletePeriodById")]
        public async Task<IActionResult> DeletePeriodById(int id)
        {
            Log.Information("DeletePeriodById:Started");
            try
            {
                var entityFromDB = await _context.TsPeriods.FindAsync(id);
                if (entityFromDB == null)
                {
                    Log.Information("DeletePeriodById:Error: product is null");
                    return NotFound();
                }
                entityFromDB.IsActive = false;
                entityFromDB.ModifyDate = DateTime.Now;
                _context.TsPeriods.Update(entityFromDB);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeletePeriodById: Unable to delete data: ", ex);
            }
            Log.Information("DeletePeriodById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }
        private bool ifEntityExists(int id)
        {
            return _context.TsPeriods.Any(e => e.Id == id);
        }
    }
}
