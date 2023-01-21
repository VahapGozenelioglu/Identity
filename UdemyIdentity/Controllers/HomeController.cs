using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UdemyIdentity.Models;
using UdemyIdentity.ViewModels;

namespace UdemyIdentity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager { get; }
        public HomeController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.PhoneNumber = newUser.PhoneNumber;

                IdentityResult result = await userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(newUser);
        }
    }
}
