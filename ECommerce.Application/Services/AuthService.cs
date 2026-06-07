using CleanAPI.Application.DTOs.Auth;
using CleanAPI.Application.Exceptions;
using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using CleanAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CleanAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenServices _tokenService;

        public AuthService(
            IUserRepository userRepository,
            ITokenServices tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            var emailExists = await _userRepository
                .EmailExistsAsync(request.Email);

            if (emailExists)
                throw new BadRequestException("Email already registered");

            if (string.IsNullOrWhiteSpace(request.Role))
                throw new BadRequestException("Role is required");

            var allowedRoles = new[] { "User", "Admin" };

            if (!allowedRoles.Contains(request.Role))
                throw new BadRequestException("Invalid Role");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role ?? "User",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _userRepository.AddAsync(user);
            var token = _tokenService.GenerateToken(created);

            return new AuthResponseDto
            {
                Token = token,
                Name = created.Name,
                Email = created.Email,
                Role = created.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new BadRequestException("Invalid email or password");

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}
