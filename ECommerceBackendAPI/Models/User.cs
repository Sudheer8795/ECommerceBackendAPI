using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required] public string PasswordHash { get; set; } = null!;
    [Required] public string Role { get; set; } = "Customer"; // Admin / Customer
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
