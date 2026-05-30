using CleanAPI.Application.Common;
using CleanAPI.Application.DTOs.Product;
using CleanAPI.Application.DTOs.Review;
using CleanAPI.Application.Exceptions;
using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Stock = p.Stock
            });
        }

        public async Task<ProductwithReviewDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetWithReviewsAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            return new ProductwithReviewDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Stock = product.Stock,
                Reviews = product.Reviews.Select(r => new ReviewResponseDto
                {
                    Id = r.Id,
                    ReviewerName = r.User?.Name?? "Unknown",
                    Comment = r.Comment,
                    Rating = r.Rating,
                    ProductId = r.ProductId
                }).ToList()
            };
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByUserIdAsync(int userId)
        {
            var products = await _productRepository.GetByUserIdAsync(userId);

            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Stock = p.Stock
            });
        }

        public async Task<ProductResponseDto> CreateAsync(
            CreateProductDto dto, int userId)
        {
            if (dto.Price <= 0)
                throw new BadRequestException("Price must be greater than 0");

            var product = new Product
            {
                ProductName = dto.ProductName,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Stock = dto.Stock,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId
            };

            var created = await _productRepository.AddAsync(product);

            return new ProductResponseDto
            {
                Id = created.Id,
                ProductName = created.ProductName,
                Price = created.Price,
                CategoryId = created.CategoryId,
                Stock = created.Stock
            };
        }

        public async Task<ProductResponseDto> UpdateAsync(
            int id, UpdateProductDto dto, int userId, string role)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            if (role != "Admin" && product.CreatedByUserId != userId)
                throw new ForbiddenException("You can only update your own products");

            if (dto.Price <= 0)
                throw new BadRequestException("Price must be greater than 0");

            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.Stock = dto.Stock;
            product.UpdatedAt = DateTime.UtcNow;

            var updated = await _productRepository.UpdateAsync(product);

            return new ProductResponseDto
            {
                Id = updated.Id,
                ProductName = updated.ProductName,
                Price = updated.Price,
                CategoryId = updated.CategoryId,
                Stock = updated.Stock
            };
        }

        public async Task DeleteAsync(int id, int userId, string role)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            if (role != "Admin" && product.CreatedByUserId != userId)
                throw new ForbiddenException("You can only delete your own products");

            await _productRepository.DeleteAsync(product);
        }

        public async Task<PagedResult<ProductResponseDto>> GetPagedAsync(ProductQueryParams queryParams)
        {
            var (products, totalCounts) = await _productRepository.GetPagedAsync(queryParams);

            var dtos = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId
            });

            return new PagedResult<ProductResponseDto>(
                dtos,
                totalCounts,
                queryParams.Page,
                queryParams.PageSize);
        }
    }
}
