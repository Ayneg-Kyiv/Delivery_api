using Domain.Interfaces.Services;
using Domain.Models.DTOs.News;
using Domain.Models.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController (IArticleService articleService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDto article, CancellationToken cancellationToken)
        {
            var result = await articleService.CreateArticleAsync(article, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleDto article, CancellationToken cancellationToken)
        {
            var result = await articleService.UpdateArticleAsync(article, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await articleService.DeleteArticleAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("block/delete/{id}")]
        public async Task<IActionResult> DeleteArticleBlock([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await articleService.DeleteArticleBlockAsync(id, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await articleService.GetArticleByIdAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetArticles(
            [FromQuery] string? author,
            [FromQuery] string? category,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await articleService.GetArticlesBatchAsync(author, category, pageNumber, pageSize, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("search-params")]
        public async Task<IActionResult> GetSearchParams(CancellationToken cancellationToken)
        {
            var result = await articleService.GetSearchParamsAsync(cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("block/create")]
        public async Task<IActionResult> AddArticleBlock([FromBody] CreateArticleBlockDto articleBlock, CancellationToken cancellationToken)
        {
            var result = await articleService.AddArticleBlockAsync(articleBlock, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPut("block/update")]
        public async Task<IActionResult> UpdateArticleBlock([FromBody] UpdateArticleBlockDto articleBlock, CancellationToken cancellationToken)
        {
            var result = await articleService.UpdateArticleBlockAsync(articleBlock, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
