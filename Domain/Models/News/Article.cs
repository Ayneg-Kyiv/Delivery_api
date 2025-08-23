using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.News
{
    public class Article : BaseModel
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
