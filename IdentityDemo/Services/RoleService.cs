using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<Response<object>> AddUserRoleAsync(string roleName)
        {
            var exists = await _roleManager.FindByNameAsync(roleName);
            if (exists != null)
                return Response<object>.Fail("Role already exists", 400);

            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            if(result.Succeeded)
                return Response<object>.Success(200);

            return Response<object>.Fail("Role could not create", 400);
        }
    }
}
