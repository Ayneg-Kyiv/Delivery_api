using Domain.Models.DTOs;
using Domain.Models.DTOs.News;
using Domain.Models.News;

namespace Domain.Interfaces.Services
{
    public interface IArticleService
    {
        Task<TResponse> GetArticlesBatch(
            string? author,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<TResponse> GetArticleById(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateArticle(CreateArticleDto article, CancellationToken cancellationToken);
        Task<TResponse> UpdateArticle(Article article, CancellationToken cancellationToken);
        Task<TResponse> DeleteArticle(Guid id, CancellationToken cancellationToken);
    }
}
