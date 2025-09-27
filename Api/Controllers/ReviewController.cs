using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto, CancellationToken cancellationToken)
        {
            var result = await reviewService.CreateReviewAsync(createReviewDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await reviewService.DeleteReviewAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await reviewService.GetReviewByIdAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsBatchByUserId(
            [FromRoute] Guid userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await reviewService.GetReviewsBatchByUserIdAsync(userId, pageNumber, pageSize, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}