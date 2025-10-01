using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.News;
using Domain.Models.News;
using Infrastructure.Contexts;
using System.Linq.Expressions;

namespace Application.Services
{
    public class ArticleService(IBaseRepository<Article, ShippingDbContext> repository,
                                IBaseRepository<ArticleBlock, ShippingDbContext> blockRepository,
                                IMapper mapper,
                                IFileService fileService) : IArticleService
    {
        private List<Expression<Func<Article, bool>>> Predicates = [];

        public async Task<TResponse> AddArticleBlockAsync(CreateArticleBlockDto articleBlock, CancellationToken cancellationToken)
        {
            if (articleBlock == null) return TResponse.Failure(405, "Article block data is null");

            if (articleBlock.ArticleId == Guid.Empty) return TResponse.Failure(405, "Invalid article ID");

            if (articleBlock.Title == null && articleBlock.Content == null && articleBlock.Image == null)
                return TResponse.Failure(405, "At least one of Title, Content, or Image must be provided");

            Predicates.Add(a => a.Id == articleBlock.ArticleId);

            var article = (await repository.FindAsync(Predicates, cancellationToken)).FirstOrDefault();
            if (article == null) return TResponse.Failure(404, "Article not found");

            var newBlock = mapper.Map<ArticleBlock>(articleBlock);
            if (articleBlock.Image != null)
                newBlock.ImagePath = await fileService.SaveFileAsync(articleBlock.Image, cancellationToken);

            var createdBlock = await blockRepository.AddAsync(newBlock, cancellationToken);

            if (createdBlock == false) return TResponse.Failure(500, "Failed to create article block");

            return TResponse.Successful(newBlock.Id, "Article block created successfully");
        }

        public async Task<TResponse> CreateArticleAsync(CreateArticleDto article, CancellationToken cancellationToken)
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

            if (!createdArticle)
                return TResponse.Failure(500, "Failed to create article");

            return TResponse.Successful(newArticle.Id, "Article created successfully");
        }

        public async Task<TResponse> DeleteArticleAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) return TResponse.Failure(405, "Invalid article ID");

            Predicates.Add(a => a.Id == id);
            var article = (await repository.FindAsync(Predicates, cancellationToken)).FirstOrDefault();

            if (article == null) return TResponse.Failure(404, "Article not found");

            if (!string.IsNullOrWhiteSpace(article.ImagePath))
            {
                fileService.DeleteFileAsync(article.ImagePath);
            }

            var deleted = await repository.DeleteAsync(article, cancellationToken);

            if (!deleted) return TResponse.Failure(500, "Failed to delete article");

            return TResponse.Successful(200, "Article deleted successfully");
        }

        public async Task<TResponse> DeleteArticleBlockAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) return TResponse.Failure(405, "Invalid article block ID");

            var block = (await blockRepository.FindAsync([ab => ab.Id == id], cancellationToken)).FirstOrDefault();

            if (block == null) return TResponse.Failure(404, "Article block not found");

            if (!string.IsNullOrWhiteSpace(block.ImagePath))
                fileService.DeleteFileAsync(block.ImagePath);

            var deleted = await blockRepository.DeleteAsync(block, cancellationToken);

            if (!deleted) return TResponse.Failure(500, "Failed to delete article block");

            return TResponse.Successful(200, "Article block deleted successfully");

        }

        public async Task<TResponse> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) return TResponse.Failure(405, "Invalid article ID");

            var article = (await repository.FindWithIncludesAsync([a => a.Id == id], cancellationToken, [a => a.ArticleBlocks]))
                .FirstOrDefault();

            if (article == null) return TResponse.Failure(404, "Article not found");

            var articleDto = mapper.Map<GetArticleDto>(article);

            return TResponse.Successful(articleDto, "Article retrieved successfully");
        }

        public async Task<TResponse> GetArticlesBatchAsync(
            string? author,
            string? category,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0) return TResponse.Failure(405, "Invalid pagination parameters");

            if (!string.IsNullOrEmpty(author))
                Predicates.Add(a => a.Author == author);

            if (!string.IsNullOrEmpty(category))
            {
                Predicates.Add(a => a.Category != null);
                Predicates.Add(a => a.Category! == category);
            }

            var totalArticles = await repository.GetTotalCountAsync(Predicates, cancellationToken);

            if (totalArticles == 0) return TResponse.Successful(new List<GetArticleDto>());

            var articles = await repository.FindWithIncludesAndPaginationAsync(
                Predicates,
                pageNumber,
                pageSize,
                cancellationToken,
                [a => a.ArticleBlocks]);

            var articlesDto = mapper.Map<List<GetArticleDto>>(articles);

            var paginationInfo = new Pagination()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = totalArticles
            };

            return TResponse.Successful(new PaginatedPage { Data = articlesDto, Pagination = paginationInfo }, "Articles retrieved successfully");
        }

        public async Task<TResponse> GetSearchParamsAsync(CancellationToken cancellationToken)
        {
            Expression<Func<Article, bool>> predicate = a => true;

            var authors = await repository.FindAllUniqueDataInPropertiesAsync([predicate], a => a.Author, cancellationToken);
            var categories = await repository.FindAllUniqueDataInPropertiesAsync([predicate], a => a.Category!, cancellationToken);

            var searchParams = new
            {
                Authors = authors,
                Categories = categories
            };

            return TResponse.Successful(searchParams, "Search parameters retrieved successfully");
        }

        public async Task<TResponse> UpdateArticleAsync(UpdateArticleDto article, CancellationToken cancellationToken)
        {
            if (article == null) return TResponse.Failure(405, "Article data is null");

            var existingArticle = (await repository.FindAsync([a => a.Id == article.Id], cancellationToken)).FirstOrDefault();
            if (existingArticle == null) return TResponse.Failure(404, "Article not found");


            existingArticle.Title = article.Title ?? "";
            existingArticle.Content = article.Content ?? "";
            existingArticle.Author = article.Author ?? "";
            existingArticle.Category = article.Category ?? "";

            existingArticle.SetUpdatedAt();

            if (article.Image != null)
            {
                if (!string.IsNullOrWhiteSpace(existingArticle.ImagePath))
                    fileService.DeleteFileAsync(existingArticle.ImagePath);

                existingArticle.ImagePath = await fileService.SaveFileAsync(article.Image, cancellationToken);
            }

            var updated = await repository.UpdateAsync(existingArticle, cancellationToken);
            if (!updated) return TResponse.Failure(500, "Failed to update article");

            return TResponse.Successful(existingArticle, "Article updated successfully");
        }

        public async Task<TResponse> UpdateArticleBlockAsync(UpdateArticleBlockDto articleBlock, CancellationToken cancellationToken)
        {
            if (articleBlock == null) return TResponse.Failure(405, "Article block data is null");

            var existingBlock = (await blockRepository.FindAsync([ab => ab.Id == articleBlock.Id], cancellationToken)).FirstOrDefault();

            if (existingBlock == null) return TResponse.Failure(404, "Article block not found");

            existingBlock.Title = articleBlock.Title ?? "";
            existingBlock.Content = articleBlock.Content ?? "";

            existingBlock.SetUpdatedAt();

            if (articleBlock.Image != null)
            {
                if (!string.IsNullOrWhiteSpace(existingBlock.ImagePath))
                    fileService.DeleteFileAsync(existingBlock.ImagePath);

                existingBlock.ImagePath = await fileService.SaveFileAsync(articleBlock.Image, cancellationToken);
            }

            var updated = await blockRepository.UpdateAsync(existingBlock, cancellationToken);

            if (!updated) return TResponse.Failure(500, "Failed to update article block");

            return TResponse.Successful(existingBlock, "Article block updated successfully");
        }
    }
}
