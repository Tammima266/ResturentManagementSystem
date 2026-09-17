using Microsoft.AspNetCore.Identity;

namespace ResturentManagementSystem1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}