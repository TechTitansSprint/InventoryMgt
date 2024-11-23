using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all roles in the system.
        /// </summary>
        /// <returns>200 OK with a list of roles, or 500 if there was an error fetching the roles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            if (roles == null) return StatusCode(500, "An error occurred while fetching roles.");
            return Ok(roles);
        }

        /// <summary>
        /// Retrieves a role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>200 OK with the role if found, or 404 if the role does not exist.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound($"Role with ID {id} not found.");
            return Ok(role);
        }

        /// <summary>
        /// Creates a new role in the system.
        /// </summary>
        /// <param name="roleDTO">The role data to be created.</param>
        /// <returns>201 Created with the created role, or 400 if the data is invalid.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null) return BadRequest("Invalid role data.");

            var createdRole = await _roleService.CreateRoleAsync(roleDTO);
            if (createdRole == null) return StatusCode(500, "An error occurred while creating the role.");

            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, createdRole);
        }

        /// <summary>
        /// Updates an existing role in the system.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="roleDTO">The role data to update the role with.</param>
        /// <returns>204 NoContent if successful, or 404 if the role is not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null) return BadRequest("Invalid role data.");

            var isUpdated = await _roleService.UpdateRoleAsync(id, roleDTO);
            if (!isUpdated) return NotFound($"Role with ID {id} not found.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a role from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>204 NoContent if deletion is successful, or 404 if the role is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var isDeleted = await _roleService.DeleteRoleAsync(id);
            if (!isDeleted) return NotFound($"Role with ID {id} not found.");

            return NoContent();
        }
    }
}
