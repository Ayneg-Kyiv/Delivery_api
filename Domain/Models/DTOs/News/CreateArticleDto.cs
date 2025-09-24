using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.News
{
    public class CreateArticleDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Category { get; set; } = null!;
        
        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }

        public List<CreateArticleBlockDto>? ArticleBlocks { get; set; }
    }
}
