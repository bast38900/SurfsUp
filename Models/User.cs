using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    // Class to create new users for Identity, within the App

    public class User
    {

        // [Required],  [RegularExpression] and  is uses as validation attributes
        // Errormessage, used to help users
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
