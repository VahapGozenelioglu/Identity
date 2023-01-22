using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyIdentity.Models;

namespace UdemyIdentity.CustomValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName)){
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password can not contain username"
                });
            }

            if (password.ToLower().Contains(user.Email))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsEmail",
                    Description = "Password cannot contain user email"
                });
            }

            if(errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
