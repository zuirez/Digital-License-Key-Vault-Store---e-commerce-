using System.ComponentModel.DataAnnotations;

namespace Digital_License___Key_Vault_Store.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please fill the password")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please put your password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match")]

        public string password { get; set; } = string.Empty;
    }
}
