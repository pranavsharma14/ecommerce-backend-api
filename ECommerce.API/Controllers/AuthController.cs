using CleanAPI.Application.DTOs.Auth;
using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanAPI.API.Extensions;

namespace CleanAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.GetUserId(),
                name = User.GetUserName(),
                email = User.GetUserEmail(),
                role = User.GetUserRole()
            });
        }
    }
}
