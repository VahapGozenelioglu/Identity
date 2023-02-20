using System.ComponentModel.DataAnnotations;

namespace UdemyIdentity.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should contain at least 4 characters")]
        [Display(Name = "Your old password")]
        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should contain at least 4 characters")]
        [Display(Name = "Your new password")]
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should contain at least 4 characters")]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
        public string ConfirmPassword { get; set; }
    }
}
