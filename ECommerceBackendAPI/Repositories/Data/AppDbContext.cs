using ECommerce.Api.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ECommerce.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.CategoryId });

        // Seed admin
        var admin = new User
        {
            Id = 1,
            Name = "Admin",
            Email = "admin@ecommerce.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
            Role = "Admin",
            CreatedDate = DateTime.UtcNow
        };
        modelBuilder.Entity<User>().HasData(admin);
        base.OnModelCreating(modelBuilder);
    }
}
