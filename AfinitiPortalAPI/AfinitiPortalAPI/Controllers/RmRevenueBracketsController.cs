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
    public class RmRevenueBracketsController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public RmRevenueBracketsController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllRevenueBrackets")]
        public async Task<ActionResult<List<RmRevenueBracketDTO>>> GetRevenueBrackets()
        {
            Log.Information("GetAllRevenueBrackets:Started");
            List<RmRevenueBracketDTO> data = new();
            try
            {
                data = await _context.RmRevenueBrackets.Where(x => x.IsActive).ProjectTo<RmRevenueBracketDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllRevenueBrackets: Unable to load data: ", ex);
            }
            Log.Information("GetAllRevenueBrackets:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetRevenueBracketById")]
        public async Task<ActionResult<RmRevenueBracket>> GetRevenueBracket(int id)
        {
            Log.Information("GetRevenueBracketById:Started");
            RmRevenueBracket rmEntity = new();
            try
            {
                rmEntity = await _context.RmRevenueBrackets.FindAsync(id);
                if (rmEntity == null)
                {
                    Log.Error("GetRevenueBracketById: Error: Product is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetRevenueBracketById: Unable to fetch data: ", ex);
            }
            Log.Information("GetRevenueBracketById:Completed");
            return rmEntity;
        }

        [HttpPatch]
        [Route("UpdateRevenueBracket")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<RmRevenueBracketDTO> patchEntity)
        {
            Log.Information("UpdateRevenueBracket:Started");
            try
            {
                var entityFromDB = await _context.RmRevenueBrackets.FindAsync(id); // Get our original person object from the database. 
                RmRevenueBracketDTO personDTO = _mapper.Map<RmRevenueBracketDTO>(entityFromDB); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(personDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(personDTO, entityFromDB); //Use automapper to map the DTO back ontop of the database object. 
                _context.RmRevenueBrackets.Update(entityFromDB); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!RevenueBracketExists(id))
                {
                    Log.Error("UpdateRevenueBracket:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateRevenueBracket:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateRevenueBracket:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateRevenueBracket:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateRevenueBracket")]
        public async Task<ActionResult<RmRevenueBracket>> CreateRevenueBracket([FromBody] RmRevenueBracketDTO revenueBracketDTO)
        {
            Log.Information("CreateRevenueBracket:Started");
            try
            {
                revenueBracketDTO.CreatedAt = DateTime.Now;
                RmRevenueBracket newEntity = new();
                newEntity = _mapper.Map<RmRevenueBracketDTO, RmRevenueBracket>(revenueBracketDTO);
                _context.RmRevenueBrackets.Add(newEntity);
                await _context.SaveChangesAsync();
                revenueBracketDTO.Id = newEntity.Id;
                Log.Information("CreateRevenueBracket:Completed");
                return CreatedAtAction("GetRevenueBracket", new { id = newEntity.Id }, revenueBracketDTO);
            }
            catch (Exception ex)
            {
                Log.Error("CreateRevenueBracket: Unable to save data: ", ex);
                throw;
            }
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRevenueBracketById")]
        public async Task<IActionResult> DeleteRevenueBracketById(int id)
        {
            Log.Information("DeleteRevenueBracketById:Started");
            try
            {
                var entityFromDB = await _context.RmRevenueBrackets.FindAsync(id);
                if (entityFromDB == null)
                {
                    Log.Information("DeleteRevenueBracketById:Error: product is null");
                    return NotFound();
                }
                entityFromDB.IsActive = false;
                entityFromDB.ModifiedAt = DateTime.Now;
                _context.RmRevenueBrackets.Update(entityFromDB);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteRevenueBracketById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteRevenueBracketById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteRevenueBracketInBulk")]
        public async Task<IActionResult> DeleteRevenueBracketInBulk(int[] ids)
        {
            Log.Information("DeleteRevenueBracketInBulk:Started");
            try
            {
                _context.RmRevenueBrackets.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifiedAt = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteRevenueBracketInBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteRevenueBracketInBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }
        private bool RevenueBracketExists(int id)
        {
            return _context.RmRevenueBrackets.Any(e => e.Id == id);
        }
    }
}
