using IdentityDemo.Dtos;
using IdentityDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto LoginDto)
        {
            var result = await _authenticationService.LoginAsync(LoginDto);
            return CreateActionResultInstance(result);
        }
    }
}
