using Microsoft.AspNetCore.Identity;

namespace SurfsUp.Models
{
    // Class for editing Roles
    // Gets role, and finds users associated with role
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }

    }
}
