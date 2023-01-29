using System.ComponentModel.DataAnnotations;

namespace UdemyIdentity.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }



        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should contain at least 4 characters")]
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }
    }
}
