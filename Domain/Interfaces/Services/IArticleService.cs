using Domain.Models.DTOs;
using Domain.Models.DTOs.News;
using Domain.Models.News;

namespace Domain.Interfaces.Services
{
    public interface IArticleService
    {
        Task<TResponse> GetArticlesBatchAsync(
            string? author,
            string? category,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<TResponse> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateArticleAsync(CreateArticleDto article, CancellationToken cancellationToken);

        Task<TResponse> AddArticleBlockAsync(CreateArticleBlockDto articleBlock, CancellationToken cancellationToken);
        Task<TResponse> UpdateArticleAsync(UpdateArticleDto article, CancellationToken cancellationToken);
        Task<TResponse> UpdateArticleBlockAsync(UpdateArticleBlockDto articleBlock, CancellationToken cancellationToken);
        Task<TResponse> DeleteArticleAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> DeleteArticleBlockAsync(Guid id, CancellationToken cancellationToken);

        Task<TResponse> GetSearchParamsAsync(CancellationToken cancellationToken);
    }
}
