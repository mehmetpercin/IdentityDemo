using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecuredData()
        {
            return Ok("Secured data");
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAdminSecuredData()
        {
            return Ok("Secured admin data");
        }
    }
}
