using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description {  get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int Stock {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
         
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
