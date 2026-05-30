using CleanAPI.Domain.Entities;
using ECommerce.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task <CartResponseDto> GetMyCartAsync(int userId);
        Task<CartResponseDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<CartItemResponseDto> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto dto);
        Task RemoveFromCartAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }
}
