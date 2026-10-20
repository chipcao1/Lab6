using System.Data.Entity;

namespace StoreMVC.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=StoreMVC")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}