using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService service) : ControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
        {
            var result = await service.GetUserDataAsync(HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
        {
            var result = await service.ChangePasswordAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("change-user-data")]
        public async Task<IActionResult> ChangeUserData([FromBody] ChangeUserDataDTO request)
        {
            var result = await service.ChangeUserDataAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("update-profile-image")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageDTO request, CancellationToken cancellationToken)
        {
            var result = await service.ChangeProfileImageAsync(request, HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            var result = await service.ForgotPasswordAsync(request, HttpContext);
            return Ok(result); // Always return 200 to prevent email enumeration
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            var result = await service.ResetPasswordAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
