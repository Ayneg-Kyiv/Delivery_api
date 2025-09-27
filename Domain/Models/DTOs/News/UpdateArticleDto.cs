using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.News
{
    public class UpdateArticleDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        [Required]
        public string Category { get; set; } = null!;

        /// <summary>
        /// Image file to be uploaded for the article.
        /// </summary>
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The article's featured image")]
        public IFormFile? Image { get; set; }
    }
}
