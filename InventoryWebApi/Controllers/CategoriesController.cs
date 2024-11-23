using InventoryWebApi.Models;
using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        // Constructor injecting the service and logger
        // Initializes the controller with ICategoryService for business logic
        // and ILogger for logging
        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        // GET: api/categories
        // Fetches all categories asynchronously
        // Logs the request and handles errors gracefully
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Received GET request to fetch all categories.");
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching categories: {ex.Message}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // GET: api/categories/{id}
        // Fetches a category by its ID
        // Returns 404 if the category is not found, or 500 if an error occurs
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Received GET request to fetch category with ID: {id}");
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching category with ID {id}: {ex.Message}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // POST: api/categories
        // Adds a new category using the provided CategoryDTO
        // Returns 201 Created on success, or 400 BadRequest on failure
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
        {
            try
            {
                _logger.LogInformation("Received POST request to add a new category.");
                bool isCreated = await _categoryService.AddCategoryAsync(categoryDTO);

                if (isCreated)
                {
                    _logger.LogInformation("Category created successfully.");
                    return CreatedAtAction("GetCategory", new { id = categoryDTO.CategoryId }, categoryDTO);
                }

                return BadRequest("Error while adding category.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the category: {ex.Message}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // PUT: api/categories/{id}
        // Updates an existing category identified by the provided ID
        // Returns 204 No Content on success, or 404 NotFound if the category doesn't exist
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
        {
            try
            {
                _logger.LogInformation($"Received PUT request to update category with ID {id}.");
                bool isUpdated = await _categoryService.UpdateCategoryAsync(id, categoryDTO);

                if (!isUpdated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating category with ID {id}: {ex.Message}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // DELETE: api/categories/{id}
        // Deletes a category identified by the provided ID
        // Returns 204 No Content on success, or 404 NotFound if the category doesn't exist
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Received DELETE request to remove category with ID {id}.");
                bool isDeleted = await _categoryService.DeleteCategoryAsync(id);

                if (!isDeleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting category with ID {id}: {ex.Message}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
