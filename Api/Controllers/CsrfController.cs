using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsrfController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCsrfToken()
        {
            return await Task.FromResult(Ok());

            // Note: application have csrf middleware, so this endpoint is just to return
            // csrf token via middleware where it needed.
        }
    }
}
