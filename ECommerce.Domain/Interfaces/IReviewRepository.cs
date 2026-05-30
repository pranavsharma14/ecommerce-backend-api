using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<Review?> GetByIdAsync(int id);
        Task<Review> AddAsync(Review review);
        Task DeleteAsync(Review review);
    }
}
