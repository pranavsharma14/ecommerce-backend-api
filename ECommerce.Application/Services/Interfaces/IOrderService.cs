using ECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId);
        Task<OrderResponseDto> GetOrderDetailAsync(int userId, int orderId);
        Task<OrderResponseDto> PlaceOrderAsync(int userId);
        Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
    }
}
