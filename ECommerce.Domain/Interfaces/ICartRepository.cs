using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<CartItem> AddItemAsync(CartItem item);
        Task<CartItem> UpdateItemAsync(CartItem item);
        Task RemoveItemAsync(CartItem item);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task ClearCartAsync(int cartId);
    }
}
