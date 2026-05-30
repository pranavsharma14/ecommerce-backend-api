using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public Cart? Carts { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
