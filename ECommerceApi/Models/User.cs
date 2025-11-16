using System;

namespace ECommerceApi.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = Roles.Customer;
        public DateTime CreatedDate { get; set; }
    }
}
