using Microsoft.AspNetCore.Identity;

namespace SurfsUpWeb.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
    }
}