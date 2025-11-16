using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwt;

        public AuthController(AppDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest("Email already registered");

            if (req.Password.Length < 6 || !req.Password.Any(char.IsDigit) || !req.Password.Any(char.IsLetter))
                return BadRequest("Password must be at least 6 chars and alphanumeric");

            var user = new User
            {
                Name = req.Name,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = Roles.Customer,
                CreatedDate = System.DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse(token, System.DateTime.UtcNow.AddMinutes(60)));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse(token, System.DateTime.UtcNow.AddMinutes(60)));
        }
    }
}
