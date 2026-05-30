using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Infrastructure.Service
{
    public class TokenService : ITokenServices
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _expiryMinutes;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"]
                ?? throw new ArgumentNullException("SecretKey not found");
            _issuer = configuration["JwtSettings:Issuer"]
                ?? throw new ArgumentNullException("Issuer not found");
            _audience = configuration["JwtSettings:Audience"]
                ?? throw new ArgumentNullException("Audience not found");
            _expiryMinutes = Convert.ToDouble(
                configuration["JwtSettings:ExpiryInMinutes"] ?? "60");
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            return handler.WriteToken(token);
        }
    }
}
