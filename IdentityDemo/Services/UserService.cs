using IdentityDemo.Dtos;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;       
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<object>> AddUserRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDto.EmailAddress);
            if(user == null)
                return Response<object>.Fail("User not found", 400);

            var result = await _userManager.AddToRolesAsync(user, userRoleDto.RoleNames);
            if(result.Succeeded)
                return Response<object>.Success(200);

            return Response<object>.Fail("Could not assign role to user", 400);
        }

        public async Task<Response<AppUserCreateResult>> CreateUserAsync(AppUserCreateDto userCreateDto)
        {
            var user = new AppUser { Email = userCreateDto.Email, UserName = userCreateDto.UserName, FirstName = userCreateDto.FirstName, LastName = userCreateDto.LastName };

            var result = await _userManager.CreateAsync(user, userCreateDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Response<AppUserCreateResult>.Fail(errors, 400);
            }

            return Response<AppUserCreateResult>.Success(new AppUserCreateResult { Id = user.Id }, 200);
        }
    }
}
