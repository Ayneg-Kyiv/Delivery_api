using Domain.Models.DTOs;
using Domain.Models.DTOs.Ride.DeliveryOffer;
using Domain.Models.DTOs.Ride.DeliveryRequest;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services
{
    public interface IDeliveryRequestService
    {
        // DeliveryRequest and DeliveryOffer methods
        Task<TResponse> GetDeliveryRequestsBatchAsync(
            string? cityFrom,
            string? cityTo,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool isPickedUp,
            bool isDelivered,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<TResponse> GetDeliveryRequestsBatchBySenderIdAsync(
            Guid? senderId,
            string? cityFrom,
            string? cityTo,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool isPickedUp,
            bool isDelivered,
            int pageNumber,
            int pageSize,
            HttpContext context,
            CancellationToken cancellationToken);
        Task<TResponse> GetDeliveryRequestByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateDeliveryRequestAsync(CreateDeliveryRequestDto request, CancellationToken cancellationToken);
        Task<TResponse> DeleteDeliveryRequestAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetRequestAsPickedUpAsync(Guid id, HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> SetRequestAsDeliveredAsync(Guid id, HttpContext context, CancellationToken cancellationToken);

        // DeliveryOffer
        Task<TResponse> GetDeliveryOffersBatchByDriverIdAsync(
            Guid? driverId,
            int pageNumber,
            int pageSize,
            HttpContext context,
            CancellationToken cancellationToken);

        Task<TResponse> GetDeliveryOfferById(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateDeliveryOfferAsync(CreateDeliveryOfferDto offer, CancellationToken cancellationToken);
        Task<TResponse> AcceptDeliveryOfferAsync(Guid offerId, HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> DeclineDeliveryOfferAsync(Guid offerId, HttpContext context, CancellationToken cancellationToken);
    }
}