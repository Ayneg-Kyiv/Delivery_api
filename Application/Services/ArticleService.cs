using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.DTOs.News;
using Domain.Models.News;
using Infrastructure.Contexts;
using System.Linq.Expressions;

namespace Application.Services
{
    public class ArticleService(IBaseRepository<Article, ShippingDbContext> repository) : IArticleService
    {
        public Task<GetArticleDto?> CreateArticle(Article article, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteArticle(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<GetArticleDto?> GetArticleById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetArticleDto>> GetArticlesBatch(int batchSize, int batchNumber)
        {
            throw new NotImplementedException();
        }

        public Task<(int TotalCount, int TotalPages)> GetTotalArticlesCount(Expression<Func<Article, bool>> predicate, CancellationToken cancellationToken, int pageSize, params Expression<Func<Article, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<GetArticleDto?> UpdateArticle(Article article, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
