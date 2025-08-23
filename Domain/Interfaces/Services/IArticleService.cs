using Domain.Models.DTOs.News;
using Domain.Models.News;
using System.Linq.Expressions;

namespace Domain.Interfaces.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<GetArticleDto>> GetArticlesBatch(int batchSize, int batchNumber);
        Task<(int TotalCount, int TotalPages)> GetTotalArticlesCount(
            Expression<Func<Article, bool>> predicate,
            CancellationToken cancellationToken,
            int pageSize,
            params Expression<Func<Article, object>>[] includes);

        Task<GetArticleDto?> GetArticleById(Guid id, CancellationToken cancellationToken);
        Task<GetArticleDto?> CreateArticle(Article article, CancellationToken cancellationToken);
        Task<GetArticleDto?> UpdateArticle(Article article, CancellationToken cancellationToken);
        Task<bool> DeleteArticle(Guid id, CancellationToken cancellationToken);
    }
}
