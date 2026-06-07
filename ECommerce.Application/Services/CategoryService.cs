using CleanAPI.Application.DTOs.Product;
using CleanAPI.Application.Exceptions;
using CleanAPI.Application.DTOs.Category;
using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanAPI.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var category = await _categoryRepository.GetAllAsync();

            return category.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.CategoryName,
                Description = c.Description
            });
        }
        public async Task<CategoryWithProductDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetWithProductAsync(id);
            
            if(category == null)
            {
                throw new NotFoundException("Category", id);
            }

            return new CategoryWithProductDto
            {
                Id = category.Id,
                Name = category.CategoryName,
                Description = category.Description,
                Products = category.Products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    CategoryId = p.CategoryId
                }).ToList()
            };
        }
        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Category name is required");

            var category = new Category
            {
                CategoryName = dto.Name,
                Description = dto.Description
            };

            var created = await _categoryRepository.AddAsync(category);

            return new CategoryResponseDto
            {
                Id = created.Id,
                Name = created.CategoryName,
                Description = created.Description,
            };
        }
        public async Task<CategoryResponseDto> UpdateAsync(UpdateCategoryDto dto, int id)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Category name is required");

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException("Category", id);

            category.CategoryName = dto.Name; 
            category.Description = dto.Description;
            
            var updated = await _categoryRepository.UpdateAsync(category);

            return new CategoryResponseDto
            {
                Id = updated.Id,
                Name = updated.CategoryName,
                Description = updated.Description
            };
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException("Category", id);

            await _categoryRepository.DeleteAsync(category);
        }
    }
}
