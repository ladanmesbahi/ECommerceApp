using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Shared.Dtos
{
    public class UserChangePassword
    {
        [Required, StringLength(maximumLength: 100, MinimumLength = 6)]
        public string Password { get; set; } = String.Empty;

        [Compare(nameof(Password), ErrorMessage = "The passwords don't match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
