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

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByNameAsync(User.Identity.Name).Result;

                bool exist = userManager.CheckPasswordAsync(user, passwordChangeViewModel.OldPassword).Result;

                if (exist)
                {
                    IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword).Result;

                    if (result.Succeeded)
                    {
                        userManager.UpdateSecurityStampAsync(user);

                        signInManager.SignOutAsync();
                        signInManager.PasswordSignInAsync(user, passwordChangeViewModel.NewPassword, true, false);
                        ViewBag.success = "true";
                    }
                    else
                    {
                        foreach(var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Your old password is wrong");
                }
            }

            return View();
        }
    }
}
