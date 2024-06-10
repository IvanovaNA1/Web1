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
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<OrderProducts> OrderProducts{ get; set; } = null!;
        public DbSet<OrderServices> OrderServices { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<Provider> Provider { get; set; } = null!;
        public DbSet<Service> Service { get; set; } = null!;
        public DbSet<Shipment> Shipment { get; set; } = null!;
        public DbSet<ShipmentList> ShipmentList { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<UserRole> UserRole { get; set; } = null!;
        public DbSet<Warehouse> Warehouse { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID);

            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.ShipmentItems) 
                .WithOne(sl => sl.Shipment) 
                .HasForeignKey(sl => sl.ShipmentID); 
            //modelBuilder.Entity<Order>()
            //    .HasMany(o => o.OrderProducts) 
            //    .WithOne() 
            //    .HasForeignKey(op => op.OrderID);
            //modelBuilder.Entity<Order>()
            //    .HasMany(o => o.OrderServices)
            //    .WithOne()
            //    .HasForeignKey(op => op.OrderID);

            base.OnModelCreating(modelBuilder);

        }
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
