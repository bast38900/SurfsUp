using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    // Class for editing Roles
    // Gets rolename and id, and add or delete users by id.
    public class RoleModification
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string[]? AddIds { get; set; }

        public string[]? DeleteIds { get; set; }
    }
}
