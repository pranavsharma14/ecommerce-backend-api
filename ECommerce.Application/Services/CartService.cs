using CleanAPI.Application.Exceptions;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Services.Interfaces;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartResponseDto> GetMyCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return new CartResponseDto
                {
                    UserId = userId,
                    CartItems = new List<CartItemResponseDto>(),
                    TotalAmount = 0
                };
            }
               

            var items = cart.CartItems.Select(item => new CartItemResponseDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.ProductName,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                SubTotal = item.Product.Price * item.Quantity,
            }).ToList();

            return new CartResponseDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = items,
                TotalAmount = items.Sum(x => x.SubTotal)
            };
        }
        public async Task<CartResponseDto> AddToCartAsync(int userId, AddToCartDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new NotFoundException("Product", dto.ProductId);

            if (product.Stock < dto.Quantity)
                throw new BadRequestException("Insufficient Stock");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                };
                await _cartRepository.CreateCartAsync(cart);
            }

           

            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, dto.ProductId);

            if (existingItem != null)
            {
                var updatedQuantity = existingItem.Quantity + dto.Quantity;

                if (product.Stock < updatedQuantity)
                    throw new BadRequestException("Insufficient stock");

                existingItem.Quantity = updatedQuantity;

                await _cartRepository.UpdateItemAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                };

                await _cartRepository.AddItemAsync(cartItem);
            }
            return await GetMyCartAsync(userId);
        }

        public async Task<CartItemResponseDto> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new NotFoundException("Cart not found");

            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new NotFoundException("Cart Item not found");

            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

            if (product.Stock < dto.Quantity)
                throw new BadRequestException("Insufficient Stock");
            
            cartItem.Quantity = dto.Quantity;

            await _cartRepository.UpdateItemAsync(cartItem);

            return new CartItemResponseDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = cartItem.Quantity,
                SubTotal = product.Price * dto.Quantity,
            };

        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                throw new NotFoundException("Cart Not Found");

            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new NotFoundException("Cart item not found");

            await _cartRepository.RemoveItemAsync(cartItem);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                throw new NotFoundException("Cart Not found");

            await _cartRepository.ClearCartAsync(cart.Id);
        }
    }
}