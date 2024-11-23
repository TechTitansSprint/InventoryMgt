using InventoryWebApi.Services.IServices;
using InventoryWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InventoryWebApi.DTO;

namespace InventoryWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(InventoryDBContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all orders from the database and returns them as a list of OrderDTOs.
        /// </summary>
        /// <returns>A list of OrderDTOs representing all orders in the database.</returns>
        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all orders.");
                var orders = await _context.Order.ToListAsync();
                return orders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching orders: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Fetches a specific order by its ID from the database and returns it as an OrderDTO.
        /// </summary>
        /// <param name="id">The ID of the order to be fetched.</param>
        /// <returns>The OrderDTO for the specified order, or null if not found.</returns>
        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching order with ID {id}");
                var order = await _context.Order.FindAsync(id);
                if (order == null) return null;

                return new OrderDTO
                {
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    OrderDate = order.OrderDate,
                    Status = order.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order with ID {id}: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Creates a new order in the database using the provided OrderDTO.
        /// </summary>
        /// <param name="orderDTO">The OrderDTO containing the order data to be created.</param>
        /// <returns>The created OrderDTO with the newly assigned OrderId, or null if an error occurs.</returns>
        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new order.");
                var order = new Order
                {
                    ProductId = orderDTO.ProductId,
                    Quantity = orderDTO.Quantity,
                    OrderDate = orderDTO.OrderDate,
                    Status = orderDTO.Status,
                    OrderId = orderDTO.OrderId
                };

                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                orderDTO.OrderId = order.OrderId;  // Update DTO with the newly created order ID
                return orderDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating order: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Updates an existing order in the database using the provided OrderDTO.
        /// </summary>
        /// <param name="id">The ID of the order to be updated.</param>
        /// <param name="orderDTO">The OrderDTO containing the updated order data.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateOrderAsync(int id, OrderDTO orderDTO)
        {
            try
            {
                _logger.LogInformation($"Updating order with ID {id}");
                var order = await _context.Order.FindAsync(id);
                if (order == null) return false;

                order.ProductId = orderDTO.ProductId;
                order.Quantity = orderDTO.Quantity;
                order.OrderDate = orderDTO.OrderDate;
                order.Status = orderDTO.Status;

                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes an existing order from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the order to be deleted.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting order with ID {id}");
                var order = await _context.Order.FindAsync(id);
                if (order == null) return false;

                _context.Order.Remove(order);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting order with ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
