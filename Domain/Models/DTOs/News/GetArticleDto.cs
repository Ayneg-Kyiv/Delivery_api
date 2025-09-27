namespace Domain.Models.DTOs.News
{
    public class GetArticleDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        
        public string Author { get; set; } = null!;
        public string? ImagePath { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<GetArticleBlockDto>? ArticleBlocks { get; set; }
    }
}
