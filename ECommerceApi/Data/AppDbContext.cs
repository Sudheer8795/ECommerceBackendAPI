using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace ECommerceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            // Soft-delete filter for products: only show active non-deleted products by default
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.DeletedDate.HasValue && p.IsActive);
            // Unique product name within a category
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.Name, p.CategoryId }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public static void Seed(AppDbContext db)
        {
            if (!db.Users.Any())
            {
                var admin = new User
                {
                    Name = "Admin",
                    Email = "admin@store.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                    Role = Roles.Admin,
                    CreatedDate = DateTime.UtcNow
                };
                db.Users.Add(admin);

                var customer = new User
                {
                    Name = "Test Customer",
                    Email = "cust@store.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Cust12345"),
                    Role = Roles.Customer,
                    CreatedDate = DateTime.UtcNow
                };
                db.Users.Add(customer);
                db.SaveChanges();
            }

            if (!db.Categories.Any())
            {
                var cat = new Category { Name = "Clothing", Description = "Apparel", CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
                db.Categories.Add(cat);
                db.SaveChanges();

                db.Products.Add(new Product
                {
                    Name = "Blue Shirt",
                    Description = "A nice blue shirt",
                    Price = 399.99m,
                    CategoryId = cat.Id,
                    StockQuantity = 50,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                });
                db.SaveChanges();
            }
        }
    }
}
