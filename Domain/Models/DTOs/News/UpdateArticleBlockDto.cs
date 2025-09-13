using Microsoft.AspNetCore.Http;

namespace Domain.Models.DTOs.News
{
    public class UpdateArticleBlockDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }
}
