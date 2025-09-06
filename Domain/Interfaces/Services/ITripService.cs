using Domain.Models.DTOs;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.DTOs.Ride.Trip;

namespace Domain.Interfaces.Services
{
    public interface ITripService
    {
        // Trips
        Task<TResponse> GetTripsBatchAsync(
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

        Task<TResponse> CreateTripAsync(CreateTripDto tripDto, CancellationToken cancellationToken);
        Task<TResponse> AddDeliverySlotAsync(Guid tripId, CreateDeliverySlotDto slot, CancellationToken cancellationToken);
        Task<TResponse> GetTripByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> UpdateLocationAsync(UpdateLocationDto locationDto, CancellationToken cancellationToken);
        Task<TResponse> DeleteTripAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetTripAsCompletedAsync(Guid id, CancellationToken cancellationToken);

        // Delivery Orders
        Task<TResponse> GetDeliveryOrdersBatchAsync(
            Guid tripId,
            Guid userId,
            CancellationToken cancellationToken);

        Task<TResponse> CreateDeliveryOrderAsync(CreateDeliveryOrderDto deliveryOrderDto, CancellationToken cancellationToken);
        Task<TResponse> DeleteDeliveryOrderAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsPickedUpAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> SetAsDeliveredAsync(Guid id, CancellationToken cancellationToken);
    }
}
