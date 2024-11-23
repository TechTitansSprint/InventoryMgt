using InventoryWebApi.Models;
using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebApi.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(InventoryDBContext context, ILogger<SupplierService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all suppliers
        /// <summary>
        /// Retrieves a list of all suppliers from the database.
        /// </summary>
        /// <returns>A collection of SupplierDTO objects representing all suppliers.</returns>
        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all suppliers.");

                // Retrieve all supplier entities from the database
                var suppliers = await _context.Supplier.ToListAsync();

                // Map supplier entities to DTOs
                var supplierDTOs = suppliers.Select(s => new SupplierDTO
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    ContactInfo = s.ContactInfo,
                    Address = s.Address
                }).ToList();

                return supplierDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching suppliers: {ex.Message}", ex);
                return null;
            }
        }

        // Get supplier by ID
        /// <summary>
        /// Retrieves a specific supplier by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to retrieve.</param>
        /// <returns>A SupplierDTO object representing the supplier, or null if not found.</returns>
        public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching supplier with ID {id}.");

                // Find the supplier by its ID
                var supplier = await _context.Supplier.FindAsync(id);
                if (supplier == null) return null;

                // Map the supplier entity to a DTO
                return new SupplierDTO
                {
                    SupplierId = supplier.SupplierId,
                    Name = supplier.Name,
                    ContactInfo = supplier.ContactInfo,
                    Address = supplier.Address
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching supplier with ID {id}: {ex.Message}", ex);
                return null;
            }
        }

        // Create a new supplier
        /// <summary>
        /// Creates a new supplier in the database.
        /// </summary>
        /// <param name="supplierDTO">The SupplierDTO object containing the supplier details to be added.</param>
        /// <returns>The created SupplierDTO object with the assigned SupplierId, or null if the operation fails.</returns>
        public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplierDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new supplier.");

                // Map the DTO to a supplier entity
                var supplier = new Supplier
                {
                    Name = supplierDTO.Name,
                    ContactInfo = supplierDTO.ContactInfo,
                    Address = supplierDTO.Address,
                    SupplierId = supplierDTO.SupplierId
                };

                // Add the new supplier to the database
                _context.Supplier.Add(supplier);
                await _context.SaveChangesAsync();

                // Update the DTO with the generated SupplierId
                supplierDTO.SupplierId = supplier.SupplierId;
                return supplierDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating supplier: {ex.Message}", ex);
                return null;
            }
        }

        // Update supplier details
        /// <summary>
        /// Updates an existing supplier's details in the database.
        /// </summary>
        /// <param name="id">The ID of the supplier to update.</param>
        /// <param name="supplierDTO">The SupplierDTO object containing updated supplier details.</param>
        /// <returns>True if the update was successful, false if the supplier was not found or an error occurred.</returns>
        public async Task<bool> UpdateSupplierAsync(int id, SupplierDTO supplierDTO)
        {
            try
            {
                _logger.LogInformation($"Updating supplier with ID {id}.");

                // Find the supplier by its ID
                var supplier = await _context.Supplier.FindAsync(id);
                if (supplier == null) return false;

                // Update supplier details
                supplier.Name = supplierDTO.Name;
                supplier.ContactInfo = supplierDTO.ContactInfo;
                supplier.Address = supplierDTO.Address;

                // Save the updated supplier to the database
                _context.Supplier.Update(supplier);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating supplier with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        // Delete supplier by ID
        /// <summary>
        /// Deletes a supplier from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to delete.</param>
        /// <returns>True if the deletion was successful, false if the supplier was not found or an error occurred.</returns>
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting supplier with ID {id}.");

                // Find the supplier by its ID
                var supplier = await _context.Supplier.FindAsync(id);
                if (supplier == null) return false;

                // Remove the supplier from the database
                _context.Supplier.Remove(supplier);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting supplier with ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
