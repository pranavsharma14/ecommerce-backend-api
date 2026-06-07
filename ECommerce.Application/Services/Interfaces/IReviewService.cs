using CleanAPI.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponseDto>> GetReviewByProductAsync(int productId);
        Task<ReviewResponseDto> AddReviewAsync(int productId, CreateReviewDto dto, int userId);
        Task DeleteReviewAsync(int productId, int reviewId, int userId, string role);
    }
}
