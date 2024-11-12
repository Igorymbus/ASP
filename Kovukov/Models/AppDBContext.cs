using Microsoft.EntityFrameworkCore;

namespace Kovukov.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Order_Items> Order_Items { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
