using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web1.Models.DBModels;

namespace Web1.Models
{
    public class MyDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Delivery> Delivery { get; set; } = null!;
        public DbSet<Icebox> Icebox { get; set; } = null!;
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<OrderProducts> OrderProduct{ get; set; } = null!;
        public DbSet<OrderServices> OrderService { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<ProductSection> ProductSection { get; set; } = null!;
        public DbSet<Provider> Provider { get; set; } = null!;
        public DbSet<Section> Section { get; set; } = null!;
        public DbSet<Service> Service { get; set; } = null!;
        public DbSet<Shipment> Shipment { get; set; } = null!;
        public DbSet<ShipmentList> ShipmentList { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<UserAccount> UserAccount { get; set; } = null!;
        public DbSet<UserRole> UserRole { get; set; } = null!;
        public DbSet<Warehouse> Warehouse { get; set; } = null!;
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            Database.EnsureDeleted();

            Category.Add(new Category() { CategoryName = "Роза" });
            Category.Add(new Category() { CategoryName = "Цветы" });

            Database.EnsureCreated();
        }
    }
}
