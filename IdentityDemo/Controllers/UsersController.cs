using IdentityDemo.Dtos;
using IdentityDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUserCreateDto appUserCreateDto)
        {
            var result = await _userService.CreateUserAsync(appUserCreateDto);
             return CreateActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(UserRoleDto userRoleDto)
        {
            var result = await _userService.AddUserRoleAsync(userRoleDto);
            return CreateActionResultInstance(result);
        }
    }
}
