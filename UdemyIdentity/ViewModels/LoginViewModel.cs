using System.ComponentModel.DataAnnotations;

namespace UdemyIdentity.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should be at least 4 characters")]
        public string Password { get; set; }
    }
}
