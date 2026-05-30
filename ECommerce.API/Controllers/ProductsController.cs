using CleanAPI.API.Extensions;
using CleanAPI.Application.Common;
using CleanAPI.Application.DTOs.Product;
using CleanAPI.Application.DTOs.Review;
using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductwithReviewDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        // GET: api/products/my-products
        [HttpGet("my-products")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetMyProducts()
        {
            var products = await _productService
                .GetByUserIdAsync(User.GetUserId());
            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductResponseDto>> Create(
            [FromBody] CreateProductDto request)
        {
            var product = await _productService
                .CreateAsync(request, User.GetUserId());
            return Created("", product);
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductResponseDto>> Update(
            int id, [FromBody] UpdateProductDto request)
        {
            var product = await _productService
                .UpdateAsync(id, request, User.GetUserId(), User.GetUserRole());
            return Ok(product);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService
                .DeleteAsync(id, User.GetUserId(), User.GetUserRole());
            return Ok("Product deleted successfully");
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetPaged(
            [FromQuery] ProductQueryParams queryParams)
        {
            var result = await _productService.GetPagedAsync(queryParams);
            return Ok(result);
        }
    }
}
