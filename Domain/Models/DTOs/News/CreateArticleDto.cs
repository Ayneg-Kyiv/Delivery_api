using Microsoft.AspNetCore.Http;

namespace Domain.Models.DTOs.News
{
    public class CreateArticleDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Author { get; set; } = null!;

        public IFormFile? Image { get; set; }
    }
}
