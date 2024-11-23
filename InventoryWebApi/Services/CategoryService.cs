using InventoryWebApi.DTO;
using InventoryWebApi.Models;
using InventoryWebApi.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryWebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<CategoryService> _logger;

        // Injecting the logger
        public CategoryService(InventoryDBContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all categories from the database and returns them as a list of CategoryDTOs.
        /// </summary>
        /// <returns>A list of CategoryDTOs representing all categories in the database.</returns>
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all categories from the database.");
                var categories = await _context.Category
                    .Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        CategoryType = c.CategoryType
                    })
                    .ToListAsync();

                _logger.LogInformation($"Fetched {categories.Count} categories.");
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching all categories: {ex.Message}", ex);
                throw new Exception("Error fetching categories", ex);
            }
        }

        /// <summary>
        /// Fetches a specific category by its ID from the database and returns it as a CategoryDTO.
        /// </summary>
        /// <param name="id">The ID of the category to be fetched.</param>
        /// <returns>The CategoryDTO for the specified category, or null if not found.</returns>
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching category with ID {id}.");
                var category = await _context.Category
                    .Where(c => c.CategoryId == id)
                    .Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        CategoryType = c.CategoryType
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                }
                else
                {
                    _logger.LogInformation($"Category with ID {id} fetched successfully.");
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching category with ID {id}: {ex.Message}", ex);
                throw new Exception($"Error fetching category with ID {id}", ex);
            }
        }

        /// <summary>
        /// Adds a new category to the database using the provided CategoryDTO.
        /// </summary>
        /// <param name="categoryDTO">The CategoryDTO containing the category data to be added.</param>
        /// <returns>True if the category was successfully added, otherwise false.</returns>
        public async Task<bool> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            try
            {
                _logger.LogInformation($"Adding new category: {categoryDTO.CategoryType}.");

                var category = new Category
                {
                    CategoryType = categoryDTO.CategoryType,
                    CategoryId = categoryDTO.CategoryId
                };

                _context.Category.Add(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Category {categoryDTO.CategoryType} added successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding category: {ex.Message}", ex);
                throw new Exception("Error adding category", ex);
            }
        }

        /// <summary>
        /// Updates an existing category in the database using the provided CategoryDTO.
        /// </summary>
        /// <param name="id">The ID of the category to be updated.</param>
        /// <param name="categoryDTO">The CategoryDTO containing the updated category data.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            try
            {
                _logger.LogInformation($"Updating category with ID {id}.");

                var category = await _context.Category.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return false;
                }

                category.CategoryType = categoryDTO.CategoryType;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Category with ID {id} updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating category with ID {id}: {ex.Message}", ex);
                throw new Exception($"Error updating category with ID {id}", ex);
            }
        }

        /// <summary>
        /// Deletes an existing category from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the category to be deleted.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting category with ID {id}.");

                var category = await _context.Category.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return false;
                }

                _context.Category.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Category with ID {id} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting category with ID {id}: {ex.Message}", ex);
                throw new Exception($"Error deleting category with ID {id}", ex);
            }
        }
    }
}
