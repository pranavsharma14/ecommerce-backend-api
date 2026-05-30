using CleanAPI.API.Extensions;
using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController: ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetMyCartAsync(int userId)
        {
            var result = await _cartService.GetMyCartAsync(User.GetUserId());
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<CartResponseDto>> AddToCartAsync([FromBody] AddToCartDto dto)
        {
            var result = await _cartService.AddToCartAsync(User.GetUserId(),dto);
            return Created("",result);
        }
        [HttpPut("{cartItemId}")]
        public async Task<ActionResult<CartItemResponseDto>> UpdateCartItemAsync(int cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            var update = await _cartService.UpdateCartItemAsync(User.GetUserId(), cartItemId, dto);
            return Ok(update);
        }
        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> RemoveFromCartAsync(int userId, int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(User.GetUserId(), cartItemId);
            return Ok(new
            {
                message = "Item Removed From Cart"
            });
        }
        [HttpDelete]
        public async Task<ActionResult> CleanCartAsync(int userId)
        {
            await _cartService.ClearCartAsync(User.GetUserId());
            return Ok(new
            {
                message = "Cart Removed"
            });
        }

    }
}
