using InventoryWebApi.DTO;
using InventoryWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryWebApi.Services.IServices
{
    public interface ICategoryService
    {
        // project
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<bool> AddCategoryAsync(CategoryDTO category);

        Task<bool> UpdateCategoryAsync(int id, CategoryDTO category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
