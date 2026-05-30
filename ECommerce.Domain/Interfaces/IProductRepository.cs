using CleanAPI.Domain.Entities;
using CleanAPI.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetWithReviewsAsync(int id);
        Task<IEnumerable<Product>> GetByUserIdAsync(int userId);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsAsync(int id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(ProductQueryParams queryParams);
    }
}
