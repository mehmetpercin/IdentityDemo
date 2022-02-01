using IdentityDemo.Dtos;
using IdentityDemo.Models;

namespace IdentityDemo.Services
{
    public interface IUserService
    {
        Task<Response<AppUserCreateResult>> CreateUserAsync(AppUserCreateDto userCreateDto);
        Task<Response<object>> AddUserRoleAsync(UserRoleDto userRoleDto);
    }
}
