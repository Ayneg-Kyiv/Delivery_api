using Domain.Models.DTOs.Support;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportChatController : ControllerBase
    {
        private readonly IXaiSupportService _supportService;

        public SupportChatController(IXaiSupportService supportService)
        {
            _supportService = supportService;
        }

        [HttpPost]
        public async Task<ActionResult<SupportChatResponse>> Post([FromBody] SupportChatRequest request, CancellationToken cancellationToken)
        {
            var reply = await _supportService.GetSupportReplyAsync(request.UserMessage, cancellationToken);
            return Ok(new SupportChatResponse { Reply = reply });
        }
    }
}
