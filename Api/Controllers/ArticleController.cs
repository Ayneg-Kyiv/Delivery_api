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
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromForm] CreateArticleDto article, CancellationToken cancellationToken)
        {
            var result = await articleService.CreateArticleAsync(article, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateArticle([FromForm] Article article, CancellationToken cancellationToken)
        {
            var result = await articleService.UpdateArticleAsync(article, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var result = await articleService.DeleteArticleAsync(id, cancellationToken);
            
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
        public async Task<IActionResult> GetArticles([FromQuery] string? author,
                                                     [FromQuery] int pageNumber = 1,
                                                     [FromQuery] int pageSize = 10,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await articleService.GetArticlesBatchAsync(author, pageNumber, pageSize, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
