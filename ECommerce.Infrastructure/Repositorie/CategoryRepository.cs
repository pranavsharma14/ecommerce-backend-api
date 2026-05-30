using CleanAPI.Infrastructure.Data;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Infrastructure.Repositorie
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.ToListAsync();
        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.FindAsync(id);
        public async Task<Category?> GetWithProductAsync(int id)
            => await _context.Categories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id);
        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistAsync(int id)
            => await _context.Categories.AnyAsync(x => x.Id == id);
    }
}
