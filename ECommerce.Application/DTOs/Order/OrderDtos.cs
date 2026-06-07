using CleanAPI.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>(); 
    }
    public class PlaceOrderDto
    {

    }
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
