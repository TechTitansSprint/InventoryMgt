using InventoryWebApi.DTO;

namespace InventoryWebApi.Services.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO);
        Task<bool> UpdateOrderAsync(int id, OrderDTO orderDTO);
        Task<bool> DeleteOrderAsync(int id);
    }
}
