using Domain.Models.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IReviewService
    {
        Task<TResponse> GetReviewsBatchByUserIdAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<TResponse> GetReviewByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateReviewAsync(CreateReviewDto createReviewDto, CancellationToken cancellationToken);
        Task<TResponse> DeleteReviewAsync(Guid id, CancellationToken cancellationToken);
    }
}
