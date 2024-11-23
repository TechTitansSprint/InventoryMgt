using InventoryWebApi.Models;
using InventoryWebApi.DTO;
using InventoryWebApi.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;

namespace InventoryWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDBContext _context;
        private readonly ILogger<ProductService> _logger;

        // Constructor to initialize the context and logger
        public ProductService(InventoryDBContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all products with their associated category details
        /// <summary>
        /// Fetches all products from the database, including their associated category.
        /// </summary>
        /// <returns>A list of ProductDTO objects containing product details.</returns>
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all products.");

                // Retrieve all products, including their related category, from the database
                var products = await _context.Product.Include(p => p.Category).ToListAsync();

                // Map the products to ProductDTOs
                var productDTOs = products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    SKU = p.SKU,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    StockLevel = p.StockLevel,
                    ReorderLevel = p.ReorderLevel,
                    SupplierID = p.SupplierID,
                }).ToList();

                return productDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}", ex);
                return null;
            }
        }

        // Get a specific product by ID
        /// <summary>
        /// Fetches a product by its ID, including the associated category.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>A ProductDTO object containing the product details, or null if not found.</returns>
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id}");

                // Retrieve the product with its related category by ID
                var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
                if (product == null) return null;

                // Map the product entity to a ProductDTO
                return new ProductDTO
                {
                    ProductId = product.ProductId,
                    SKU = product.SKU,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    StockLevel = product.StockLevel,
                    ReorderLevel = product.ReorderLevel,
                    SupplierID = product.SupplierID

                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching product with ID {id}: {ex.Message}", ex);
                return null;
            }
        }

        // Create a new product
        /// <summary>
        /// Creates a new product and saves it to the database.
        /// </summary>
        /// <param name="productDTO">The ProductDTO object containing the new product's details.</param>
        /// <returns>The created ProductDTO with the assigned ProductId.</returns>
        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new product.");

                // Map the ProductDTO to a Product entity
                var product = new Product
                {
                    ProductId = productDTO.ProductId,
                    SKU = productDTO.SKU,
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryId = productDTO.CategoryId,
                    StockLevel = productDTO.StockLevel,
                    ReorderLevel = productDTO.ReorderLevel,
                    SupplierID= productDTO.SupplierID
                };

                // Add the new product to the database and save changes
                _context.Product.Add(product);
                await _context.SaveChangesAsync();

                // Update the DTO with the newly generated ProductId
                productDTO.ProductId = product.ProductId;
                return productDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}", ex);
                return null;
            }
        }

        // Update an existing product
        /// <summary>
        /// Updates an existing product's details in the database.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDTO">The ProductDTO object containing the updated product details.</param>
        /// <returns>True if the product was updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateProductAsync(int id, ProductDTO productDTO)
        {
            try
            {
                _logger.LogInformation($"Updating product with ID {id}");

                // Find the product by its ID
                var product = await _context.Product.FindAsync(id);
                if (product == null) return false;

                // Update the product's details
                product.SKU = productDTO.SKU;
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;
                product.CategoryId = productDTO.CategoryId;
                product.StockLevel = productDTO.StockLevel;
                product.ReorderLevel = productDTO.ReorderLevel;
                product.SupplierID = productDTO.SupplierID;

                // Save the updated product to the database
                _context.Product.Update(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        // Delete a product by ID
        /// <summary>
        /// Deletes a product by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>True if the product was deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID {id}");

                // Find the product by its ID
                var product = await _context.Product.FindAsync(id);
                if (product == null) return false;

                // Remove the product from the database
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
