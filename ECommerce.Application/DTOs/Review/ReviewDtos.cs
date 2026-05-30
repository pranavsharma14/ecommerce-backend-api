using CleanAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.DTOs.Review
{
    public class CreateReviewDto
    {
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public ProductRatings Ratings { get; set; }
    }

    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public ProductRatings Rating { get; set; }
        public int ProductId { get; set; }
    }


}
