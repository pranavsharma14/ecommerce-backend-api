using CleanAPI.API.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanAPI.Application.DTOs.Category;
using CleanAPI.Application.Services.Interfaces;
namespace CleanAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryservice;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryservice = categoryService;
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var categories = await _categoryservice.GetAllAsync();
            return Ok(categories);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithProductDto>> GetById(int id)
        {
            var categories = await _categoryservice.GetByIdAsync(id);
            return Ok(categories);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CreateCategoryDto category)
        {
            var created = await _categoryservice.CreateAsync(category);
            return Created("",created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> Update([FromBody] UpdateCategoryDto category, int id)
        {
            var updated = await _categoryservice.UpdateAsync(category, id);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryservice.DeleteAsync(id);
            return Ok("Category Delete successfully");
        }
    }
}
