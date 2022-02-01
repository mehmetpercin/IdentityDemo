using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Validators
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Şifre alanı kullanıcı adı içeremez" });
            }
            if (password.ToLower().Contains("1234"))
            {
                errors.Add(new IdentityError { Code = "PasswordContains1234", Description = "Şifre alanı ardışık sayı içeremez" });
            }
            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsEmail", Description = "Şifre alanı email içeremez" });
            }
            if (!errors.Any())
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
