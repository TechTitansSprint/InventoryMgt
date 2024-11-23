using InventoryWebApi.DTO;

namespace InventoryWebApi.Services.IServices
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplierDTO);
        Task<bool> UpdateSupplierAsync(int id, SupplierDTO supplierDTO);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
