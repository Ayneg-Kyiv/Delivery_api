using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.News
{
    public class CreateArticleBlockDto
    {
        public Guid? ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }


        /// <summary>
        /// Image associated with the article block.
        /// </summary>
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The article's block featured image")]
        public IFormFile? Image { get; set; }
    }
}
