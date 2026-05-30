    using CleanAPI.API.Extensions;
    using ECommerce.Application.DTOs.Order;
    using ECommerce.Application.Services.Interfaces;
    using ECommerce.Domain.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace ECommerce.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class OrderController : ControllerBase
        {
            private readonly IOrderService _orderService;

            public OrderController(IOrderService orderService)
            {
                _orderService = orderService;
            }
            [HttpGet]
            public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetMyOrdersAsync()
            {
                var result = await _orderService.GetMyOrdersAsync(User.GetUserId());
                return Ok(result);
            }
            [HttpGet("{orderId}")]
            public async Task<ActionResult<OrderResponseDto>> GetOrderDetailAsync(int orderId)
            {
                var resultd = await _orderService.GetOrderDetailAsync(User.GetUserId(), orderId);
                return Ok(resultd);
            }
            [HttpPost]
            public async Task<ActionResult<OrderResponseDto>> PlaceOrderAsync()
            {
                var placed = await _orderService.PlaceOrderAsync(User.GetUserId());
                return Ok(placed);
            }
            [HttpPut("{orderId}")]
            public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatusAsync(int orderId, [FromBody] UpdateOrderStatusDto dto)
            {
                var update = await _orderService.UpdateOrderStatusAsync(orderId, dto);
                return Ok(update);
            }
            [HttpGet("all")]
            [Authorize(Roles ="Admin")]
            public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
            {
                 var result = await _orderService.GetAllOrdersAsync();
                 return Ok(result);
            }
        }
    }
