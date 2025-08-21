using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] SigninDTO request)
        {
            var result = await authService.SigninAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignupDTO request)
        {
            var result = await authService.SignupAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            var result = await authService.ConfirmEmailAsync(token, email, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> Signout()
        {
            var result = await authService.SignoutAsync(HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("refresh-session")]
        public async Task<IActionResult> RefreshSession()
        {
            var result = await authService.RefreshSessionAsync(HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("check-is-email-exists/{email}")]
        public async Task<IActionResult> CheckIsEmailExists([FromRoute]string email)
        {
            var result = await authService.CheckIsUserExists(email);

             return Ok(result);
        }
    }
}
