using InventoryWebApi.DTO;

namespace InventoryWebApi.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task<RoleDTO> CreateRoleAsync(RoleDTO roleDTO);
        Task<bool> UpdateRoleAsync(int id, RoleDTO roleDTO);
        Task<bool> DeleteRoleAsync(int id);
    }
}
