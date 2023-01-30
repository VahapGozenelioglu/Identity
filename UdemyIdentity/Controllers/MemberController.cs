using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentity.Models;
using Mapster;
using UdemyIdentity.ViewModels;

namespace UdemyIdentity.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        public UserManager<AppUser> userManager { get; set; }
        public SignInManager<AppUser> signInManager { get; set; }

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
 
        public IActionResult Index()
        {
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;

            var userViewModel = user.Adapt<UserViewModel>();

            return View(userViewModel);
        }
    }
}
