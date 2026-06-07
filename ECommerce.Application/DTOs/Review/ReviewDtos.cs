using CleanAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.DTOs.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Comment is Required")]
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage ="Rating should be between 1 and 5")]
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
