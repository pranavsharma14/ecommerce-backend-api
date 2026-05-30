using CleanAPI.Application.Common;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using CleanAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Infrastructure.Repositorie
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products.FindAsync(id);

        public async Task<Product?> GetWithReviewsAsync(int id)
            => await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetByUserIdAsync(int userId)
            => await _context.Products
                .Where(p => p.CreatedByUserId == userId)
                .ToListAsync();

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Products.AnyAsync(p => p.Id == id);

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(ProductQueryParams queryParams)
        {
            var query = _context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(p =>
                p.ProductName != null &&
                p.ProductName.ToLower().Contains(search) ||
                p.Category != null &&
                p.Category.CategoryName.ToLower().Contains(search));
            }

            if(queryParams.CategoryId.HasValue)
            {
                query = query.Where(p =>
                p.CategoryId == queryParams.CategoryId);
            }

            if(queryParams.MinPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price >= queryParams.MinPrice.Value);
            }

            if (queryParams.MaxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price <= queryParams.MaxPrice.Value);
            }

            var totalCount = await query.CountAsync();

            query = queryParams.SortBy.ToLower() switch
            {
                "name" => queryParams.SortOrder == "desc" ?
                query.OrderByDescending(p => p.ProductName) :
                query.OrderBy(p => p.ProductName),

                "price" => queryParams.SortOrder == "desc" ?
                query.OrderByDescending(p => p.Price) :
                query.OrderBy(p => p.Price),

                "category" => queryParams.SortOrder == "desc" ?
                query.OrderByDescending(p => p.Category) :
                query.OrderBy(p => p.Category),

                _ => query.OrderBy(p => p.Id)
            };

            var products = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}