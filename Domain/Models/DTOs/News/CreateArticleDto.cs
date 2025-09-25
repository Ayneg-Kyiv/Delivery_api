using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.News
{
    public class CreateArticleDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        [Required]
        public string Category { get; set; } = null!;

        /// <summary>
        /// Image representing the article.
        /// </summary>
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The article's featured image")]
        public IFormFile? Image { get; set; }

        public List<CreateArticleBlockDto>? ArticleBlocks { get; set; }
    }
}
