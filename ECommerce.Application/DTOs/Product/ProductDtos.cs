using CleanAPI.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock {  get; set; }
    }

    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
    }

    public class UpdateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }

    }

    public class ProductwithReviewDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public List<ReviewResponseDto> Reviews { get; set; } = new List<ReviewResponseDto>();
    }
}
