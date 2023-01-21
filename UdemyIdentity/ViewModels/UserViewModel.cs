using System.ComponentModel.DataAnnotations;

namespace UdemyIdentity.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage ="Email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
