using CleanAPI.Application.DTOs.Review;
using CleanAPI.Application.Exceptions;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using ECommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;

        public ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewByProductAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new NotFoundException("Product", productId);

            var review = await _reviewRepository.GetByProductIdAsync(productId);

            if(!review.Any())
                throw new NotFoundException("No Review found for this product");

            return review.Select(r => new ReviewResponseDto
            {
                Id = r.Id,
                ReviewerName = r.User?.Name ?? "Unknown",
                Comment = r.Comment,
                Rating = r.Rating,
                ProductId = productId,
            }).ToList();
        }
        public async Task<ReviewResponseDto> AddReviewAsync(int productId, CreateReviewDto dto, int userId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new NotFoundException("Product", productId);

            var review = new Review
            {
                Comment = dto.Comment,
                Rating = dto.Ratings,
                CreatedAt = DateTime.UtcNow,
                ProductId = productId,
                UserId = userId
            };

            var created = await _reviewRepository.AddAsync(review);

            var reviewWithUser = await _reviewRepository.GetByIdWithUserAsync(created.Id);

            return new ReviewResponseDto
            {
                Id = created.Id,
                ReviewerName = reviewWithUser?.User?.Name ?? "Unknown",
                Comment = created.Comment,
                Rating = created.Rating,
                ProductId = productId,
            };
        }
        public async Task DeleteReviewAsync(int productId, int reviewId, int userId, string role)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
                throw new NotFoundException("Reviews not found");

            if (review.ProductId != productId)
                throw new NotFoundException("Review not found for this product");

            if (role != "Admin" && review.UserId != userId)
                throw new ForbiddenException("You can only delete your own reviews");

            await _reviewRepository.DeleteAsync(review);
        }
    }
}
