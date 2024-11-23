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
    public class UserService : IUserService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(InventoryDBContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users along with their associated roles from the database.
        /// </summary>
        /// <returns>A list of UserDTO objects representing all users, or null in case of an error.</returns>
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _context.User.Include(u => u.Role).ToListAsync();

                return users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PasswordHash = u.PasswordHash,
                    RoleId = u.RoleId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching users: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a specific user by their ID, including their associated role.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A UserDTO object representing the user, or null if the user does not exist or an error occurs.</returns>
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching user with ID {id}.");
                var user = await _context.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null) return null;

                return new UserDTO
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    RoleId = user.RoleId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user with ID {id}: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="userDTO">The UserDTO object containing user details to create.</param>
        /// <returns>The created UserDTO object with the assigned UserId, or null in case of an error.</returns>
        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new user.");

                // Validate RoleId
                var roleExists = await _context.Role.AnyAsync(r => r.RoleId == userDTO.RoleId);
                if (!roleExists)
                {
                    _logger.LogError($"RoleId {userDTO.RoleId} does not exist in the Role table.");
                    throw new Exception($"Invalid RoleId: {userDTO.RoleId}");
                }

                // Create a User entity from the DTO
                var user = new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.PasswordHash,
                    RoleId = userDTO.RoleId,
                    UserId = userDTO.UserId // Assuming you manually set UserId
                };

                // Add the user to the database
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Update the DTO with the newly created UserId
                userDTO.UserId = user.UserId;
                return userDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Updates an existing user in the database with new details.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDTO">The UserDTO object containing updated user details.</param>
        /// <returns>True if the user was successfully updated, or false if the user does not exist or an error occurs.</returns>
        public async Task<bool> UpdateUserAsync(int id, UserDTO userDTO)
        {
            try
            {
                _logger.LogInformation($"Updating user with ID {id}.");
                var user = await _context.User.FindAsync(id);

                if (user == null) return false;

                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.Email = userDTO.Email;
                user.PasswordHash = userDTO.PasswordHash;
                user.RoleId = userDTO.RoleId;

                _context.User.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes an existing user from the database.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, or false if the user does not exist or an error occurs.</returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID {id}.");
                var user = await _context.User.FindAsync(id);

                if (user == null) return false;

                _context.User.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
