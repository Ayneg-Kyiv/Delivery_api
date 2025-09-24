using Microsoft.AspNetCore.Http;

namespace Domain.Models.DTOs.News
{
    public class UpdateArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Category { get; set; } = null!;

        public IFormFile? Image { get; set; }
    }
}
