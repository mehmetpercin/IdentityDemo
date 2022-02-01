using IdentityDemo.Models;

namespace IdentityDemo.Services
{
    public interface IRoleService
    {
        Task<Response<object>> AddUserRoleAsync(string roleName);
    }
}
