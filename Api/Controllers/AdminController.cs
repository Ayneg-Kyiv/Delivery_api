using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpGet("panel-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminPanelData()
        {
            var result = await adminService.GetAdminPanelDataAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("driver-applications")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDriversApplication(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await adminService.GetDriversApplicationAsync(pageNumber, pageSize, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("approve-driver-application/{applicationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveDriverApplication([FromRoute] Guid applicationId)
        {
            var result = await adminService.ApproveDriverApplicationAsync(applicationId, default);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reject-driver-application/{applicationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectDriverApplication([FromRoute] Guid applicationId, CancellationToken cancellationToken = default)
        {
            var result = await adminService.RejectDriverApplicationAsync(applicationId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}