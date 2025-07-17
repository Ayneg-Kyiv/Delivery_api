using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService service) : ControllerBase
    {
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
        {
            var result = await service.ChangePasswordAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("change-user-data")]
        public async Task<IActionResult> ChangeUserData([FromBody] ChangeUserDataDTO request)
        {
            var result = await service.ChangeUserDataAsync(request, HttpContext);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
