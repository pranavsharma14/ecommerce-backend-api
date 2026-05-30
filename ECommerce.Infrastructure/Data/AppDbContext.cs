using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanAPI.Infrastructure.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product → User relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review → User relationship
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cart → User relationship
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Carts)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order → User relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItems → Product relationship
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(c => c.Product)
            //    .WithMany()
            //    .HasForeignKey(c => c.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Product → Categories relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItems → Product relationship
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(o => o.Product)
            //    .WithMany()
            //    .HasForeignKey(o => o.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // OrderItems → Order relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItems → Cart relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review → Product relationship - No
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Price precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.PriceAtTime)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<int>();
        }
    }
}

