using InventoryWebApi.Models;
using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryWebApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<RoleService> _logger;

        public RoleService(InventoryDBContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all roles
        /// <summary>
        /// Retrieves a list of all roles from the database.
        /// </summary>
        /// <returns>A collection of RoleDTO objects representing all roles.</returns>
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all roles.");

                // Retrieve all roles from the database
                var roles = await _context.Role.ToListAsync();

                // Map role entities to RoleDTO objects
                return roles.Select(r => new RoleDTO
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching roles: {ex.Message}", ex);
                return null;
            }
        }

        // Get role by ID
        /// <summary>
        /// Retrieves a specific role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>A RoleDTO object representing the role, or null if not found.</returns>
        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching role with ID {id}.");

                // Find the role by its ID
                var role = await _context.Role.FindAsync(id);

                // Return null if role is not found
                if (role == null) return null;

                // Map the role entity to a RoleDTO object
                return new RoleDTO
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching role with ID {id}: {ex.Message}", ex);
                return null;
            }
        }

        // Create a new role
        /// <summary>
        /// Creates a new role in the database.
        /// </summary>
        /// <param name="roleDTO">The RoleDTO object containing the role details to be added.</param>
        /// <returns>The created RoleDTO object with the assigned RoleId, or null if the operation fails.</returns>
        public async Task<RoleDTO> CreateRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new role.");

                // Generate the new RoleId by finding the max existing RoleId and incrementing it by 1
                var newRoleId = (_context.Role.Max(r => (int?)r.RoleId) ?? 0) + 1;

                // Map the DTO to a Role entity
                var role = new Role
                {
                    RoleId = newRoleId,
                    RoleName = roleDTO.RoleName
                };

                // Add the new role to the database
                _context.Role.Add(role);
                await _context.SaveChangesAsync();

                // Update the DTO with the newly created RoleId
                roleDTO.RoleId = role.RoleId;
                return roleDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating role: {ex.Message}", ex);
                return null;
            }
        }

        // Update role details
        /// <summary>
        /// Updates an existing role's details in the database.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="roleDTO">The RoleDTO object containing updated role details.</param>
        /// <returns>True if the update was successful, false if the role was not found or an error occurred.</returns>
        public async Task<bool> UpdateRoleAsync(int id, RoleDTO roleDTO)
        {
            try
            {
                _logger.LogInformation($"Updating role with ID {id}.");

                // Find the role by its ID
                var role = await _context.Role.FindAsync(id);

                // Return false if role is not found
                if (role == null) return false;

                // Update role details
                role.RoleName = roleDTO.RoleName;

                // Save the updated role to the database
                _context.Role.Update(role);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        // Delete role by ID
        /// <summary>
        /// Deletes a role from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>True if the deletion was successful, false if the role was not found or an error occurred.</returns>
        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting role with ID {id}.");

                // Find the role by its ID
                var role = await _context.Role.FindAsync(id);

                // Return false if role is not found
                if (role == null) return false;

                // Remove the role from the database
                _context.Role.Remove(role);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting role with ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
