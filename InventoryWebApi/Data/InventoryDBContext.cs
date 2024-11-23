using Microsoft.EntityFrameworkCore;

namespace InventoryWebApi.Models
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext() { }

        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options) { }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<Supplier> Supplier { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<Category> Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
=> optionsBuilder.UseSqlServer("Data Source=.\\MSSQLSERVER01;Initial Catalog=InventoryDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    }
}
