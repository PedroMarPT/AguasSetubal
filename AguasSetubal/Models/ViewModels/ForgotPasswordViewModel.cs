using System.ComponentModel.DataAnnotations;

namespace YourNamespace.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

