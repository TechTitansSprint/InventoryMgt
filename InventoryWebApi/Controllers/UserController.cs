using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all users in the system.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null) return StatusCode(500, "An error occurred while fetching users.");
            return Ok(users);
        }

        /// <summary>
        /// Gets a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user details or a NotFound response if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound($"User with ID {id} not found.");
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="userDTO">The data for the new user to create.</param>
        /// <returns>The created user with a CreatedAtAction response.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null) return BadRequest("Invalid user data.");

            var createdUser = await _userService.CreateUserAsync(userDTO);
            if (createdUser == null) return StatusCode(500, "An error occurred while creating the user.");

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

        /// <summary>
        /// Updates an existing user in the system.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDTO">The new data for the user.</param>
        /// <returns>NoContent if the update was successful, or an error response if failed.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null) return BadRequest("Invalid user data.");

            var isUpdated = await _userService.UpdateUserAsync(id, userDTO);
            if (!isUpdated) return NotFound($"User with ID {id} not found.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>NoContent if the deletion was successful, or an error response if failed.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted) return NotFound($"User with ID {id} not found.");

            return NoContent();
        }
    }
}
