using CleanAPI.Application.DTOs.Product;
using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class  CategoryWithProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description {get ; set; }
        public List<ProductResponseDto> Products { get; set; } = new List<ProductResponseDto>();
    }
}
