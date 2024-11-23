using Microsoft.EntityFrameworkCore;

namespace InventoryWebApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StockLevel { get; set; }
        public int ReorderLevel { get; set; }

        public int? SupplierID { get; set; }

        public virtual Category Category { get; set; }
    }
}
