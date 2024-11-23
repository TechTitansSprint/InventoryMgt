using InventoryWebApi.Services.IServices;
using InventoryWebApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // Constructor to inject the IOrderService to handle order-related business logic
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        // Retrieves all orders asynchronously
        // Returns a 404 if no orders are found or 500 if there's an error
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);  // Returns 200 OK with the list of orders
        }

        // GET: api/orders/{id}
        // Retrieves a specific order by its ID
        // Returns a 404 if the order is not found
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");  // Returns 404 if order is not found
            }
            return Ok(order);  // Returns 200 OK with the found order
        }

        // POST: api/orders
        // Creates a new order from the provided OrderDTO
        // Returns 201 Created on success or 400 BadRequest if data is invalid
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest("Invalid order data.");  // Returns 400 if order data is invalid
            }

            var createdOrder = await _orderService.CreateOrderAsync(orderDTO);
            if (createdOrder == null)
            {
                return StatusCode(500, "An error occurred while creating the order.");  // Returns 500 if creation fails
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);  // Returns 201 Created with location of the new order
        }

        // PUT: api/orders/{id}
        // Updates an existing order identified by its ID
        // Returns 204 NoContent on success, 400 if ID mismatch, or 404 if not found
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null || id != orderDTO.OrderId)
            {
                return BadRequest("Order ID mismatch.");  // Returns 400 if ID doesn't match
            }

            var updated = await _orderService.UpdateOrderAsync(id, orderDTO);
            if (!updated)
            {
                return NotFound($"Order with ID {id} not found.");  // Returns 404 if order not found
            }

            return NoContent();  // Returns 204 NoContent on successful update
        }

        // DELETE: api/orders/{id}
        // Deletes an order by its ID
        // Returns 204 NoContent on success, 404 if order not found
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted)
            {
                return NotFound($"Order with ID {id} not found.");  // Returns 404 if order not found
            }

            return NoContent();  // Returns 204 NoContent on successful deletion
        }
    }
}
