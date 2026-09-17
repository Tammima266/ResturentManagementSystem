using System.ComponentModel.DataAnnotations;

namespace ResturentManagementSystem1.Models
{
    public class LoginViewModel
    {
        [Required, Display(Name = "Username or Email")]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required, Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required, Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required, EmailAddress, Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        [Display(Name = "Confirm Email")]
        public string ConfirmEmail { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}