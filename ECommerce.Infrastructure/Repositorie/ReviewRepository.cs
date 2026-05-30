using CleanAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanAPI.Domain.Interfaces;
using CleanAPI.Infrastructure.Data;

namespace CleanAPI.Infrastructure.Repositorie
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
            => await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();

        public async Task<Review?> GetByIdAsync(int id)
            => await _context.Reviews.FindAsync(id);

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
