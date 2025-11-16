using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs
{
    public record RegisterRequest([Required] string Name, [Required][EmailAddress] string Email, [Required] string Password);
    public record LoginRequest([Required][EmailAddress] string Email, [Required] string Password);
    public record AuthResponse(string Token, System.DateTime Expires);
}
