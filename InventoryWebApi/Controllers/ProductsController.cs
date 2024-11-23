using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor to inject the IProductService to handle product-related business logic
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        // Retrieves all products asynchronously
        // Returns 404 if no products are found or 500 if there's an error
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");  // Returns 404 if no products are found
            }
            return Ok(products);  // Returns 200 OK with the list of products
        }

        // GET: api/products/{id}
        // Retrieves a specific product by its ID
        // Returns 404 if the product is not found
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");  // Returns 404 if product is not found
            }
            return Ok(product);  // Returns 200 OK with the found product
        }

        // POST: api/products
        // Creates a new product from the provided ProductDTO
        // Returns 201 Created on success or 400 BadRequest if data is invalid
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Invalid product data.");  // Returns 400 if product data is invalid
            }

            var createdProduct = await _productService.CreateProductAsync(productDTO);
            if (createdProduct == null)
            {
                return StatusCode(500, "An error occurred while creating the product.");  // Returns 500 if creation fails
            }

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);  // Returns 201 Created with location of the new product
        }

        // PUT: api/products/{id}
        // Updates an existing product identified by its ID
        // Returns 204 NoContent on success, 400 if data is invalid, or 404 if product not found
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            if (productDTO == null || id != productDTO.ProductId)
            {
                return BadRequest("Invalid product data or ID mismatch.");  // Returns 400 if data is invalid or ID doesn't match
            }

            var updated = await _productService.UpdateProductAsync(id, productDTO);
            if (!updated)
            {
                return NotFound($"Product with ID {id} not found.");  // Returns 404 if product is not found
            }

            return NoContent();  // Returns 204 NoContent on successful update
        }

        // DELETE: api/products/{id}
        // Deletes a product by its ID
        // Returns 204 NoContent on success or 404 if product not found
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return StatusCode(500, "An error occurred while deleting the product.");  // Returns 500 if deletion fails
            }

            return NoContent();  // Returns 204 NoContent on successful deletion
        }
    }
}
