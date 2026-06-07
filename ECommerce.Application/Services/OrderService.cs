using CleanAPI.Application.Exceptions;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Services.Interfaces;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        private static OrderResponseDto MapOrder(Order order)
        {
            var response = new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    PriceAtTime = item.PriceAtTime,
                    SubTotal = item.PriceAtTime * item.Quantity
                }).ToList()
            };

            return response;
        }
        public async Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapOrder);
        }
        public async Task<OrderResponseDto> GetOrderDetailAsync(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order", orderId);
            if (order.UserId != userId)
                throw new ForbiddenException("You can only view your own orders");
            return MapOrder(order);
        }
        public async Task<OrderResponseDto> PlaceOrderAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new NotFoundException("Cart not Found");

            if (!cart.CartItems.Any())
                throw new BadRequestException("Cart is empty");

            foreach (var item in cart.CartItems)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException("Cart item quantity must be greater than 0");

                if(item.Product.Stock < item.Quantity)
                    throw new BadRequestException($"{item.Product.ProductName} is out of Stock");
            }
            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalAmount = cart.CartItems.Sum(x => x.Quantity * x.Product.Price),
                CreatedAt = DateTime.UtcNow,
            };
            await _orderRepository.ExecuteInTransactionAsync(async () =>
            {
                foreach (var item in cart.CartItems)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PriceAtTime = item.Product.Price
                    });

                    item.Product.Stock -= item.Quantity;
                    await _productRepository.UpdateAsync(item.Product);
                }

                order = await _orderRepository.CreateOrderAsync(order);
                await _cartRepository.ClearCartAsync(cart.Id);
            });

            return MapOrder(order);
        }
        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order", orderId);

            order.Status = dto.Status;

            await _orderRepository.UpdateOrderAsync(order);

            var updated = await _orderRepository.GetOrderByIdAsync(orderId);
            return MapOrder(updated!);
        }
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var order = await _orderRepository.GetAllOrdersAsync();

            return order.Select(MapOrder);
        }
    }
}
