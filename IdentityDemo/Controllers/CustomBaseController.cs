using IdentityDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        public static IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
