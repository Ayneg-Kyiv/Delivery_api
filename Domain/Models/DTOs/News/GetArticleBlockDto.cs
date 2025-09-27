namespace Domain.Models.DTOs.News
{
    public class GetArticleBlockDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
    }
}
