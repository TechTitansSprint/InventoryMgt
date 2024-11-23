using InventoryWebApi.Models;
using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Gets all suppliers in the system.
        /// </summary>
        /// <returns>A list of all suppliers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            if (suppliers == null)
            {
                return NotFound("No suppliers found.");
            }
            return Ok(suppliers);
        }

        /// <summary>
        /// Gets a specific supplier by their ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to retrieve.</param>
        /// <returns>The supplier details or a NotFound response if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound($"Supplier with ID {id} not found.");
            }
            return Ok(supplier);
        }

        /// <summary>
        /// Creates a new supplier in the system.
        /// </summary>
        /// <param name="supplierDTO">The data for the new supplier to create.</param>
        /// <returns>The created supplier with a CreatedAtAction response.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
            {
                return BadRequest("Invalid supplier data.");
            }

            var createdSupplier = await _supplierService.CreateSupplierAsync(supplierDTO);
            if (createdSupplier == null)
            {
                return StatusCode(500, "An error occurred while creating the supplier.");
            }

            return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.SupplierId }, createdSupplier);
        }

        /// <summary>
        /// Updates an existing supplier in the system.
        /// </summary>
        /// <param name="id">The ID of the supplier to update.</param>
        /// <param name="supplierDTO">The new data for the supplier.</param>
        /// <returns>NoContent if the update was successful, or an error response if failed.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
            {
                return BadRequest("Invalid supplier data.");
            }

            var isUpdated = await _supplierService.UpdateSupplierAsync(id, supplierDTO);
            if (!isUpdated)
            {
                return StatusCode(500, "An error occurred while updating the supplier.");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a supplier by their ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to delete.</param>
        /// <returns>NoContent if the deletion was successful, or an error response if failed.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var isDeleted = await _supplierService.DeleteSupplierAsync(id);
            if (!isDeleted)
            {
                return StatusCode(500, "An error occurred while deleting the supplier.");
            }

            return NoContent();
        }
    }
}
