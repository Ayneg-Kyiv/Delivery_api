using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAccountService accountService) : ControllerBase
    {
        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] SigninDTO request)
        {
            var result = await accountService.SigninAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignupDTO request)
        {
            var result = await accountService.SignupAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> Signout()
        {
            var result = await accountService.SignoutAsync(HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("refresh-session")]
        public async Task<IActionResult> RefreshSession()
        {
            var result = await accountService.RefreshSessionAsync(HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
