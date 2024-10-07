using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("New Password")]
        public string Confirm { get; set; }

    }
}
