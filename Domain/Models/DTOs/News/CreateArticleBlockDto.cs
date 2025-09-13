using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.News
{
    public class CreateArticleBlockDto
    {
        public Guid? ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }


        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }
    }
}
