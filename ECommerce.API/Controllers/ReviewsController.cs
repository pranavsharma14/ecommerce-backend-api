using CleanAPI.API.Extensions;
using CleanAPI.Application.DTOs.Review;
using CleanAPI.Domain.Entities;
using ECommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviewsByProduct(int productId)
        {
            var review = await _reviewService.GetReviewByProductAsync(productId);
            return Ok(review);
        }

        [HttpPost("{productId}/reviews")]
        [Authorize]
        public async Task<ActionResult<ReviewResponseDto>> AddReviewAsync(int productId, [FromBody] CreateReviewDto dto)
        {
            var created = await _reviewService.AddReviewAsync(productId, dto, User.GetUserId());
            return Created("",created);
        }

        [HttpDelete("{productId}/reviews/{reviewId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReviewAsync(int productId, int reviewId)
        {
            await _reviewService.DeleteReviewAsync(productId, reviewId, User.GetUserId(), User.GetUserRole());
            return Ok("Review Deleted Successfully");
        }
    }
}
