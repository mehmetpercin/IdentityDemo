using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Validators
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (digits.Contains(user.UserName[0]))
            {
                errors.Add(new IdentityError { Code = "UserNameFirstLetterContainsDigit", Description = "Kullanıcı adı rakamla başlayamaz" });
            }
            if (!errors.Any())
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
