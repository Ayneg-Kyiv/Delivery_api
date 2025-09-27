using Application.DTOs.Vehicles;
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
        [HttpGet("short/{id}")]
        public async Task<IActionResult> GetShortUserData([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.GetShortUserDataAsync(id, cancellationToken);

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
        [Consumes("multipart/form-data")]
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

        // --- Missing methods from IAccountService ---

        [Authorize(Roles = "User")]
        [Consumes("multipart/form-data")]
        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle([FromForm] CreateVehicleDto request, CancellationToken cancellationToken)
        {
            var result = await service.AddVehicleAsync(request, HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("update-vehicle")]
        public async Task<IActionResult> UpdateVehicle([FromForm] UpdateVehicleDto request, CancellationToken cancellationToken)
        {
            var result = await service.UpdateVehicleAsync(request, HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("delete-vehicle/{id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.DeleteVehicleAsync(id, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user-vehicles")]
        public async Task<IActionResult> GetUserVehicles(CancellationToken cancellationToken)
        {
            var result = await service.GetUserVehiclesAsync(HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("driver-required-data")]
        public async Task<IActionResult> GetDriverRequiredData(CancellationToken cancellationToken)
        {
            var result = await service.ReturnDriverRequiredData(HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [Consumes("multipart/form-data")]
        [HttpPost("set-driver-required-data")]
        public async Task<IActionResult> SetDriverRequiredData([FromForm]AddDriverDataDto dataDto, CancellationToken cancellationToken)
        {
            var result = await service.SetDriverRequiredData(dataDto.PhoneNumber, dataDto.Image, dataDto.ProfileImage, HttpContext, cancellationToken);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}