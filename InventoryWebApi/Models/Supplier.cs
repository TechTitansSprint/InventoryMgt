namespace InventoryWebApi.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
