using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}