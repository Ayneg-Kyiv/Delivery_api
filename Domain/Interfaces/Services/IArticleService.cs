using Domain.Models.DTOs;
using Domain.Models.DTOs.News;
using Domain.Models.News;

namespace Domain.Interfaces.Services
{
    public interface IArticleService
    {
        Task<TResponse> GetArticlesBatchAsync(
            string? author,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<TResponse> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateArticleAsync(CreateArticleDto article, CancellationToken cancellationToken);
        Task<TResponse> UpdateArticleAsync(Article article, CancellationToken cancellationToken);
        Task<TResponse> DeleteArticleAsync(Guid id, CancellationToken cancellationToken);
    }
}
