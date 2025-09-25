using Domain.Models.DTOs;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.DTOs.Ride.Trip;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services
{
    public interface ITripService
    {
        // Trip and DeliveryOrder methods
        Task<TResponse> GetAllUniqueLocations(CancellationToken cancellationToken);
        Task<TResponse> GetTripsBatchAsync(
            bool isCompleteed,
            string? cityFrom,
            string? cityTo,
            decimal? priceFrom,
            decimal? priceTo,
            DateTime? dateFrom,
            DateTime? dateTo,
            double? driverRatingFrom,
            string? cargoType,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<TResponse> GetTripsBatchByDriverIdWithOrdersAsync(
            Guid? id,
            bool isCompleteed,
            string? cityFrom,
            string? cityTo,
            decimal? priceFrom,
            decimal? priceTo,
            DateTime? dateFrom,
            DateTime? dateTo,
            double? driverRatingFrom,
            string? cargoType,
            int pageNumber,
            int pageSize,
            HttpContext context,
            CancellationToken cancellationToken);
        Task<TResponse> GetTripDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> CreateTripAsync(CreateTripDto tripDto, CancellationToken cancellationToken);
        Task<TResponse> AddDeliverySlotAsync(Guid tripId, CreateDeliverySlotDto slot, CancellationToken cancellationToken);
        Task<TResponse> GetTripByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> GetTripByIdWithOrdersAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> UpdateLocationAsync(UpdateLocationDto locationDto, CancellationToken cancellationToken);
        Task<TResponse> DeleteTripAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetTripAsStartedAsync(Guid tripId, CancellationToken cancellationToken);
        Task<TResponse> SetTripAsCompletedAsync(Guid id, CancellationToken cancellationToken);

        // Delivery Orders
        Task<TResponse> GetDeliveryOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> GetDeliveryOrdersBatchAsync(
            Guid tripId,
            Guid userId,
            CancellationToken cancellationToken);
        Task<TResponse> GetDeliveryOrdersBatchBySenderIdAsync(
            Guid? tripId,
            Guid? userId,
            int pageNumber,
            int pageSize,
            HttpContext context,
            CancellationToken cancellationToken);
        Task<TResponse> CreateDeliveryOrderAsync(CreateDeliveryOrderDto deliveryOrderDto, CancellationToken cancellationToken);
        Task<TResponse> DeleteDeliveryOrderAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsAcceptedAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsDeclinedAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsPickedUpAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsDeliveredAsync(Guid id, CancellationToken cancellationToken);
    }
}
