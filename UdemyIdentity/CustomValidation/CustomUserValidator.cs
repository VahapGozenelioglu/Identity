using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyIdentity.Models;

namespace UdemyIdentity.CustomValidation
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            var digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            
            foreach(var digit in digits)
            {
                if (user.UserName[0].ToString() == digit)
                {
                    errors.Add(new IdentityError
                    {
                        Code = "UsernameStartsWithDigit",
                        Description = "Username cannot start with digit"
                    });
                }
            }

            if(errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
