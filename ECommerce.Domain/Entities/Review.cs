using CleanAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        //public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public ProductRatings Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
