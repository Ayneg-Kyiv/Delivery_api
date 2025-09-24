using Domain.Models.Abstract;
using System.Text.Json.Serialization;

namespace Domain.Models.News
{
    public class ArticleBlock: BaseModel
    {
        public Guid ArticleId { get; set; }
        [JsonIgnore]
        public virtual Article Article { get; set; } = null!;

        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
    }
}
