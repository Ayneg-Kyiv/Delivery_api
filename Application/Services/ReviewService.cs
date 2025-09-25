using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Reviews;
using Domain.Models.Identity;
using Domain.Models.Reviews;
using Domain.Models.Ride;
using Infrastructure.Contexts;

namespace Application.Services
{
    public class ReviewService(
        IBaseRepository<Review, ShippingDbContext> reviewRepository,
        IBaseRepository<Trip, ShippingDbContext> tripRepository,
        IBaseRepository<DeliveryOffer, ShippingDbContext> deliveryOfferRepository,
        IBaseRepository<ApplicationUser, IdentityDbContext> userRepository,
        IMapper mapper
    ) : IReviewService
    {
        public async Task<TResponse> CreateReviewAsync(CreateReviewDto createReviewDto, CancellationToken cancellationToken)
        {
            if (createReviewDto == null)
                return TResponse.Failure(400, "Review data must be provided.");

            var review = mapper.Map<Review>(createReviewDto);

            var created = await reviewRepository.AddAsync(review, cancellationToken);

            if (!created)
                return TResponse.Failure(500, "Failed to create review.");

            var user = await userRepository.FindAsync([u => u.Id == createReviewDto.UserId], cancellationToken);

            var allReviews = await reviewRepository.FindAsync([r => r.UserId == createReviewDto.UserId], cancellationToken);

            if (user != null && user.Any() && allReviews != null && allReviews.Any())
            {
                var averageRating = allReviews.Average(r => r.Rating);
                var roundedAverage = Math.Round(averageRating, 1);
                var userToUpdate = user.First();
                userToUpdate.Rating = float.Parse(roundedAverage.ToString());
                await userRepository.UpdateAsync(userToUpdate, cancellationToken);
            }

            return TResponse.Successful(review, "Review created successfully.");
        }

        public async Task<TResponse> DeleteReviewAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Review ID must be provided.");

            var reviews = await reviewRepository.FindAsync([r => r.Id == id], cancellationToken);

            if (reviews == null || !reviews.Any())
                return TResponse.Failure(404, "Review not found.");

            var deleted = await reviewRepository.DeleteAsync(reviews.First(), cancellationToken);

            if (!deleted)
                return TResponse.Failure(500, "Failed to delete review.");

            var user = await userRepository.FindAsync([u => u.Id == reviews.First().UserId], cancellationToken);

            var allReviews = await reviewRepository.FindAsync([r => r.UserId == reviews.First().UserId], cancellationToken);

            if (user != null && user.Any() && allReviews != null && allReviews.Any())
            {
                var averageRating = allReviews.Average(r => r.Rating);
                var roundedAverage = Math.Round(averageRating, 1);
                var userToUpdate = user.First();
                userToUpdate.Rating = float.Parse(roundedAverage.ToString());
                await userRepository.UpdateAsync(userToUpdate, cancellationToken);
            }
            else if (user != null && user.Any() && (allReviews == null || !allReviews.Any()))
            {
                var userToUpdate = user.First();
                userToUpdate.Rating = 0;
                await userRepository.UpdateAsync(userToUpdate, cancellationToken);
            }

            return TResponse.Successful(deleted, "Review deleted successfully.");
        }

        public async Task<TResponse> GetReviewByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Review ID must be provided.");

            var reviews = await reviewRepository.FindAsync([r => r.Id == id], cancellationToken);

            if (reviews == null || !reviews.Any())
                return TResponse.Failure(404, "Review not found.");

            var reviewDto = mapper.Map<ReviewDto>(reviews.First());

            return TResponse.Successful(reviewDto, "Review retrieved successfully.");
        }

        public async Task<TResponse> GetReviewsBatchByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
                return TResponse.Failure(400, "User ID must be provided.");

            var totalCount = await reviewRepository.GetTotalCountAsync([r => r.UserId == userId], cancellationToken);

            var reviews = await reviewRepository.FindWithIncludesAndPaginationAsync(
                [r => r.UserId == userId],
                pageNumber,
                pageSize,
                cancellationToken,
                []
            );

            var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);

            foreach (var review in reviewDtos)
            {
                var reviewer = await userRepository.FindAsync([u => u.Id == review.ReviewerId], cancellationToken);
                if (reviewer != null && reviewer.Any())
                {
                    review.Reviewer = mapper.Map<GetApplicationUserForTripDto>(reviewer.First());
                }
            }

            var pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            var paginatedPage = new PaginatedPage
            {
                Pagination = pagination,
                Data = reviewDtos
            };

            return TResponse.Successful(paginatedPage, "Reviews retrieved successfully.");
        }
    }
}