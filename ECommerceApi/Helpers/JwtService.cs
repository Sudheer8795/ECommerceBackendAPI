using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApi.Models;

namespace ECommerceApi.Helpers
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config) { _config = config; }

        public string GenerateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt.GetValue<string>("Key") ?? throw new InvalidOperationException("JWT key missing"));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwt.GetValue<string>("Issuer"),
                audience: jwt.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.GetValue<int>("ExpireMinutes")),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
