using Project.Api.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult Token()
        {
            var token = TokenService.GenerateToken("", new[] { "" });
            return Ok(token);
        }


    }
}
