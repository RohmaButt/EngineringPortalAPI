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
using Microsoft.Data.SqlClient;

namespace AfinitiPortalAPI.Controllers
{
    public class RmProductsController : BaseController
    {
        private readonly PortalDBContext _context;
        private readonly IMapper _mapper;
        public RmProductsController(PortalDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult<List<RmProductDTO>>> GetProducts()
        {
            Log.Information("GetAllProducts:Started");
            List<RmProductDTO> data = new();
            try
            {
                data = await _context.RmProducts.Where(x => x.IsActive).ProjectTo<RmProductDTO>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("GetAllProducts: Unable to load data: ", ex);
            }
            Log.Information("GetAllProducts:Completed");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetProductById")]
        public async Task<ActionResult<RmProduct>> GetProduct(int id)
        {
            Log.Information("GetProductById:Started");
            RmProduct rmProduct = new();
            try
            {
                rmProduct = await _context.RmProducts.FindAsync(id);
                if (rmProduct == null)
                {
                    Log.Error("GetProductById: Error: Product is NULL");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetProductById: Unable to fetch data: ", ex);
            }
            Log.Information("GetProductById:Completed");
            return rmProduct;
        }

        [HttpPatch]
        [Route("UpdateProduct")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<RmProductDTO> patchEntity)
        {
            Log.Information("UpdateProduct:Started");
            try
            {
                var entityFromDB = await _context.RmProducts.FindAsync(id); // Get our original person object from the database. 
                RmProductDTO personDTO = _mapper.Map<RmProductDTO>(entityFromDB); //Use Automapper to map that to our DTO object. 
                patchEntity.ApplyTo(personDTO, ModelState); //Apply the patch to that DTO. 
                _mapper.Map(personDTO, entityFromDB); //Use automapper to map the DTO back ontop of the database object. 
                _context.RmProducts.Update(entityFromDB); //Update our person in the database. 
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    Log.Error("UpdateProduct:NotFound", ex);
                    return NotFound();
                }
                else
                {
                    Log.Error("UpdateProduct:DbUpdateConcurrencyException", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateProduct:DbUpdateConcurrencyException", ex);
            }
            Log.Information("UpdateProduct:Completed");
            return Ok();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<RmProduct>> CreateProduct([FromBody] RmProductDTO rmProductDTO)
        {
            Log.Information("CreateProduct:Started");
            try
            {
                rmProductDTO.CreatedAt = DateTime.Now;
                RmProduct rmProduct = new();
                rmProduct = _mapper.Map<RmProductDTO, RmProduct>(rmProductDTO);
                _context.RmProducts.Add(rmProduct);
                await _context.SaveChangesAsync();
                rmProductDTO.Id = rmProduct.Id;
                Log.Information("CreateProduct:Completed");
                return CreatedAtAction("GetProduct", new { id = rmProduct.Id }, rmProductDTO);
            }
            catch (Exception ex)
            {
                Log.Error("CreateProduct: Unable to save data: ", ex);
                throw;
            }
            Log.Information("CreateProduct:Completed");
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            Log.Information("DeleteProductById:Started");
            try
            {
                var entityFromDB = await _context.RmProducts.FindAsync(id);
                if (entityFromDB == null)
                {
                    Log.Information("DeleteProductById:Error: product is null");
                    return NotFound();
                }
                entityFromDB.IsActive = false;
                entityFromDB.ModifiedAt = DateTime.Now;
                _context.RmProducts.Update(entityFromDB);//Soft deletion, not hard deletion
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteProductById: Unable to delete data: ", ex);
            }
            Log.Information("DeleteProductById:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        [HttpPost]
        [Route("DeleteProductInBulk")]
        public async Task<IActionResult> DeleteProductInBulk(int[] ids)
        {
            Log.Information("DeleteProductInBulk:Started");
            try
            {
                _context.RmProducts.Where(x => ids.Contains(x.Id)).ToList().ForEach(f => { f.IsActive = false; f.ModifiedAt = DateTime.Now; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("DeleteProductInBulk: Unable to delete data: ", ex);
            }
            Log.Information("DeleteProductInBulk:Completed");
            return NoContent();//204==> No Content success status response code indicates that a request has succeeded, but that the client doesn't need to navigate away from its current page.
        }

        private bool ProductExists(string name)
        {
            return _context.RmProducts.Any(e => e.Name == name);
        }

        private bool ProductExists(int id)
        {
            return _context.RmProducts.Any(e => e.Id == id);
        }
    }
}
