using Microsoft.AspNetCore.Identity;

namespace SurfsUp.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
