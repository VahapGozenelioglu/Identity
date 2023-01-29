using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UdemyIdentity.Models;
using UdemyIdentity.ViewModels;

namespace UdemyIdentity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager { get; }
        private SignInManager<AppUser> signInManager { get; }
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(userLogin.Email);

                if (user != null)
                {
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Your account is locked for a while. Please try again later.");
                        return View(userLogin);
                    }


                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member");
                    }

                    else
                    {
                        await userManager.AccessFailedAsync(user);

                        int failCount = await userManager.GetAccessFailedCountAsync(user);

                        if (failCount == 3)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
                            ModelState.AddModelError("", "Your account is locked for 30 minutes due to 3 unsuccessful attempts");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid email or password");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(userLogin);
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
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(newUser);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)

        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;

            if (user != null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme); ;

                Services.PasswordReset.PasswordResetSendEmail(passwordResetLink);
                ViewBag.status = "Success";
            }

            else
            {
                ModelState.AddModelError("", "No user found with this email");
            }



            return View(passwordResetViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("NewPassword")]PasswordResetViewModel passwordResetViewModel)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            var user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.NewPassword);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    ViewBag.status = "Success";
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            else
            {
                ModelState.AddModelError("", "An error occurred please try again later");
            }

            return View(passwordResetViewModel);
        }
    }
}
