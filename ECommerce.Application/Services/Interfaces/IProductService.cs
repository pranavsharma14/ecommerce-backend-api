using CleanAPI.Application.Common;
using CleanAPI.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductwithReviewDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetByUserIdAsync(int userId);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto, int userId);
        Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, int userId, string role);
        Task DeleteAsync(int id, int userId, string role);
        Task<PagedResult<ProductResponseDto>> GetPagedAsync(ProductQueryParams queryParams);
    }
}
