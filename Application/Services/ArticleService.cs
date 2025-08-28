using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.News;
using Domain.Models.News;
using Infrastructure.Contexts;

namespace Application.Services
{
    public class ArticleService(IBaseRepository<Article, ShippingDbContext> repository,
                                IMapper mapper,
                                IFileService fileService) : IArticleService
    {
        public async Task<TResponse> CreateArticle(CreateArticleDto article, CancellationToken cancellationToken)
        {
            if (article == null) return TResponse.Failure(405, "Article data is null");

            if (string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Content))
                return TResponse.Failure(405, "Title and Content are required");

            var newArticle = mapper.Map<Article>(article);

            if (article.Image != null)
            {
                newArticle.ImagePath = await fileService.SaveFileAsync(article.Image, cancellationToken);
            }

            var createdArticle = await repository.AddAsync(newArticle, cancellationToken);

            return TResponse.Successful(createdArticle, "Article created successfully");
        }

        public async Task<TResponse> DeleteArticle(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) return TResponse.Failure(405, "Invalid article ID");

            var article = (await repository.FindAsync(a => a.Id == id, cancellationToken)).FirstOrDefault();

            if (article == null) return TResponse.Failure(404, "Article not found");

            if (!string.IsNullOrWhiteSpace(article.ImagePath))
            {
                fileService.DeleteFileAsync(article.ImagePath);
            }

            var deleted = await repository.DeleteAsync(article, cancellationToken);

            if (!deleted) return TResponse.Failure(500, "Failed to delete article");

            return TResponse.Successful(200, "Article deleted successfully");
        }

        public async Task<TResponse> GetArticleById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) return TResponse.Failure(405, "Invalid article ID");

            var article = (await repository.FindAsync(a => a.Id == id, cancellationToken)).FirstOrDefault();

            if (article == null) return TResponse.Failure(404, "Article not found");

            var articleDto = mapper.Map<GetArticleDto>(article);

            return TResponse.Successful(articleDto, "Article retrieved successfully");
        }

        public async Task<TResponse> GetArticlesBatch(
            string? author,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0) return TResponse.Failure(405, "Invalid pagination parameters");

            var totalArticles = await repository.GetTotalCountAsync(
                (author == null || string.IsNullOrWhiteSpace(author))
                    ? a => true
                    : a => a.Author.Contains(author),
                cancellationToken);

            if (totalArticles == 0) return TResponse.Failure(404, "No articles found");

            var articles = await repository.FindWithIncludesAndPaginationAsync(
                (author == null || string.IsNullOrWhiteSpace(author))
                    ? a => true
                    : a => a.Author.Contains(author),
                pageNumber,
                pageSize,
                cancellationToken);

            var articlesDto = mapper.Map<List<GetArticleDto>>(articles);

            var paginationInfo = new Pagination()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = totalArticles
            };

            return TResponse.Successful(new PaginatedPage { Data = articlesDto, Pagination = paginationInfo}, "Articles retrieved successfully");
        }

        public async Task<TResponse> UpdateArticle(Article article, CancellationToken cancellationToken)
        {
            if (article == null) return TResponse.Failure(405, "Article data is null");

            var existingArticle = (await repository.FindAsync(a => a.Id == article.Id, cancellationToken)).FirstOrDefault();
            if (existingArticle == null) return TResponse.Failure(404, "Article not found");

            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.Author = article.Author;

            var updated = await repository.UpdateAsync(existingArticle, cancellationToken);
            if (!updated) return TResponse.Failure(500, "Failed to update article");

            return TResponse.Successful(existingArticle, "Article updated successfully");
        }
    }
}
