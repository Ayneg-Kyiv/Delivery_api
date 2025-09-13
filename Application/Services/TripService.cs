using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.DTOs.Ride.Trip;
using Domain.Models.Identity;
using Domain.Models.Ride;
using Infrastructure.Contexts;
using System.Linq.Expressions;

namespace Application.Services
{
    public class TripService(IBaseRepository<Trip, ShippingDbContext> tripRepository,
                             IBaseRepository<DeliveryOrder, ShippingDbContext> deliveryOrderRepository,
                             IBaseRepository<DeliverySlot, ShippingDbContext> deliverySlotRepository,
                             IBaseRepository<Location, ShippingDbContext> locationRepository,
                             IBaseRepository<ApplicationUser, IdentityDbContext> userRepository,
                             IMapper mapper) : ITripService
    {
        public async Task<TResponse> GetTripsBatchAsync(
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
            CancellationToken cancellationToken)
        {
            List<Expression<Func<Trip, bool>>> predicate = [];

            if (!string.IsNullOrEmpty(cityFrom))
                predicate.Add(t => t.StartLocation.City == cityFrom);
            if (!string.IsNullOrEmpty(cityTo))
                predicate.Add(t => t.EndLocation.City == cityTo);
            if (dateFrom != null)
                predicate.Add(t => t.StartLocation.DateTime == dateFrom);
            if (dateTo != null)
                predicate.Add(t => t.EndLocation.DateTime == dateTo);
            if (priceFrom != null)
                predicate.Add(t => t.Slots.Any(s => s.ApproximatePrice >= priceFrom));
            if (priceTo != null)
                predicate.Add(t => t.Slots.Any(s => s.ApproximatePrice <= priceTo));
            if (cargoType != null)
                predicate.Add(t => t.Slots.Any(s => s.CargoSlotTypeName == cargoType));

            var totalCount = await tripRepository.GetTotalCountAsync(predicate, cancellationToken,
                [
                    t => t.StartLocation,
                    t => t.EndLocation,
                    t => t.Slots
                ]);

            var trips = await tripRepository.FindWithIncludesAndPaginationAsync(
                predicate,
                pageNumber,
                pageSize,
                cancellationToken,
                [
                    t => t.StartLocation,
                    t => t.EndLocation,
                    t => t.Slots
                ]);

            var tripDtos = mapper.Map<List<GetTripDto>>(trips);

            var pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            var PaginatedPage = new PaginatedPage
            {
                Pagination = pagination,
                Data = tripDtos
            };

            return TResponse.Successful(PaginatedPage, "Trips retrieved successfully.");
        }
        public async Task<TResponse> CreateTripAsync(CreateTripDto tripDto, CancellationToken cancellationToken)
        {
            if (tripDto == null)
                return TResponse.Failure(400, "Trip data must be provided.");

            if (tripDto.StartLocation == null)
                return TResponse.Failure(400, "Start location must be provided.");

            if (tripDto.EndLocation == null)
                return TResponse.Failure(400, "End location must be provided.");

            if (tripDto.Slots == null || tripDto.Slots.Count == 0)
                return TResponse.Failure(400, "At least one delivery slot must be provided.");

            // Validate and create start location

            var startLocation = mapper.Map<Location>(tripDto.StartLocation);
            var createdStartLocation = await locationRepository.AddAsync(startLocation, cancellationToken);
            if (createdStartLocation == false)
                return TResponse.Failure(500, "Failed to create start location.");

            var endLocation = mapper.Map<Location>(tripDto.EndLocation);
            var createdEndLocation = await locationRepository.AddAsync(endLocation, cancellationToken);
            if (createdEndLocation == false)
                return TResponse.Failure(500, "Failed to create end location.");

            var trip = mapper.Map<Trip>(tripDto);

            trip.StartLocationId = startLocation.Id;
            trip.EndLocationId = endLocation.Id;

            var createdTrip = await tripRepository.AddAsync(trip, cancellationToken);
            if (createdTrip == false)
                return TResponse.Failure(500, "Failed to create trip.");

            // Create delivery slots
            foreach (var slotDto in tripDto.Slots)
            {
                var slot = mapper.Map<DeliverySlot>(slotDto);
                slot.TripId = trip.Id;

                slot.MaxVolume = AvailableCargoSlotTypes.Types
                    .Where(cs => cs.Key == slot.CargoSlotTypeName).Select(cs => cs.Value.MaxVolume).First();
                slot.MaxWeight = AvailableCargoSlotTypes.Types
                    .Where(cs => cs.Key == slot.CargoSlotTypeName).Select(cs => cs.Value.MaxWeight).First();

                var createdSlot = await deliverySlotRepository.AddAsync(slot, cancellationToken);
                if (createdSlot == false)
                    return TResponse.Failure(500, "Failed to create delivery slot.");
            }

            var returnTrip = await tripRepository.FindWithIncludesAsync([t => t.Id == trip.Id], cancellationToken,
                [t => t.StartLocation, t => t.EndLocation, t => t.Slots]);

            var tripResultDto = mapper.Map<GetTripDto>(returnTrip.First());

            return TResponse.Successful(tripResultDto, "Trip created successfully.");
        }

        public async Task<TResponse> GetTripByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var trip = await tripRepository.FindWithIncludesAsync([t => t.Id == id], cancellationToken,
                [t => t.StartLocation, t => t.EndLocation, t => t.Slots, t => t.Orders]);

            if (trip == null || !trip.Any())
                return TResponse.Failure(404, "Trip not found.");

            var tripDto = mapper.Map<GetTripDto>(trip.First());

            return TResponse.Successful(tripDto, "Trip retrieved successfully.");
        }

        public async Task<TResponse> DeleteTripAsync(Guid id, CancellationToken cancellationToken)
        {
            var trip = await tripRepository.FindAsync([t => t.Id == id], cancellationToken);

            if (trip == null || !trip.Any())
                return TResponse.Failure(404, "Trip not found.");

            // Optionally, you might want to delete associated locations and slots
            var orders = await deliveryOrderRepository.FindAsync([o => o.TripId == trip.First().Id], cancellationToken);

            if(orders.Any(o => o.IsPickedUp || o.IsDelivered))
                return TResponse.Failure(400, "Cannot delete trip with active delivery orders.");

            await deliveryOrderRepository.DeleteBatchAsync(orders, cancellationToken);

            var slots = await deliverySlotRepository.FindAsync([s => s.TripId == trip.First().Id], cancellationToken);
            await deliverySlotRepository.DeleteBatchAsync(slots, cancellationToken);

            var startLocations = await locationRepository.FindAsync([l => l.Id == trip.First().StartLocationId], cancellationToken);
            await locationRepository.DeleteBatchAsync(startLocations, cancellationToken);

            var endLocations = await locationRepository.FindAsync([l => l.Id == trip.First().EndLocationId], cancellationToken);
            await locationRepository.DeleteBatchAsync(endLocations, cancellationToken);

            var deleted = await tripRepository.DeleteAsync(trip.First(), cancellationToken);

            if (deleted == false)
                return TResponse.Failure(500, "Failed to delete trip.");

            return TResponse.Successful(deleted, "Trip deleted successfully.");
        }

        public async Task<TResponse> UpdateLocationAsync(UpdateLocationDto locationDto, CancellationToken cancellationToken)
        {
            if (locationDto == null)
                return TResponse.Failure(400, "Location data must be provided.");

            if(locationDto.Id == Guid.Empty)
                return TResponse.Failure(400, "Location ID must be provided.");

            var existingLocation = await locationRepository.FindAsync([l => l.Id == locationDto.Id], cancellationToken);

            if (existingLocation == null || !existingLocation.Any())
                return TResponse.Failure(404, "Location not found.");

            var location = existingLocation.First();

            // Update only the fields that are provided (not null)
            location.Country = locationDto.Country ?? location.Country;
            location.City = locationDto.City ?? location.City;
            location.Address = locationDto.Address ?? location.Address;
            location.DateTime = locationDto.DateTime ?? location.DateTime;
            location.Latitude = locationDto.Latitude ?? location.Latitude;
            location.Longitude = locationDto.Longitude ?? location.Longitude;

            var updatedLocation = await locationRepository.UpdateAsync(location, cancellationToken);

            if (updatedLocation == false)
                return TResponse.Failure(500, "Failed to update location.");

            return TResponse.Successful(updatedLocation, "Location updated successfully.");
        }

        public async Task<TResponse> AddDeliverySlotAsync(Guid tripId, CreateDeliverySlotDto slot, CancellationToken cancellationToken)
        {
            var trip = await tripRepository.FindAsync([t => t.Id == tripId], cancellationToken);

            if (trip == null || !trip.Any())
                return TResponse.Failure(404, "Trip not found.");

            var deliverySlot = mapper.Map<DeliverySlot>(slot);
            deliverySlot.TripId = tripId;

            var createdSlot = await deliverySlotRepository.AddAsync(deliverySlot, cancellationToken);

            if (createdSlot == false)
                return TResponse.Failure(500, "Failed to create delivery slot.");

            return TResponse.Successful(createdSlot, "Delivery slot created successfully.");
        }

        public async Task<TResponse> CreateDeliveryOrderAsync(CreateDeliveryOrderDto deliveryOrderDto, CancellationToken cancellationToken)
        {
            if (deliveryOrderDto == null)
                return TResponse.Failure(404, "Delivery order data must be provided.");

            var user = await userRepository.FindAsync([u => u.Id == deliveryOrderDto.SenderId], cancellationToken);
            if (user == null || !user.Any())
                return TResponse.Failure(404, "User not found.");

            var trip = await tripRepository.FindAsync([t => t.Id == deliveryOrderDto.TripId], cancellationToken);
            if (trip == null || !trip.Any())
                return TResponse.Failure(404, "Trip not found.");

            var slot = await deliverySlotRepository.FindAsync([s => s.Id == deliveryOrderDto.DeliverySlotId], cancellationToken);
            if (slot == null || !slot.Any())
                return TResponse.Failure(404, "Delivery slot not found.");


            var deliveryOrder = mapper.Map<DeliveryOrder>(deliveryOrderDto);
            var createdOrder = await deliveryOrderRepository.AddAsync(deliveryOrder, cancellationToken);

            if (createdOrder == false)
                return TResponse.Failure(500, "Failed to create delivery order.");

            slot.First().IsAvailable = false;

            var updatedSlot = await deliverySlotRepository.UpdateAsync(slot.First(), cancellationToken);
            if (updatedSlot == false)
                return TResponse.Failure(500, "Failed to update delivery slot.");

            return TResponse.Successful(createdOrder, "Delivery order created successfully.");
        }

        public async Task<TResponse> DeleteDeliveryOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Delivery order ID must be provided.");

            var order = await deliveryOrderRepository.FindAsync([o => o.Id == id], cancellationToken);

            if (order == null || !order.Any())
                return TResponse.Failure(404, "Delivery order not found.");

            var slot = await deliverySlotRepository.FindAsync([s => s.Id == order.First().DeliverySlotId], cancellationToken);
            if (slot == null || !slot.Any())
                return TResponse.Failure(404, "Associated delivery slot not found.");

            var deleted = await deliveryOrderRepository.DeleteAsync(order.First(), cancellationToken);
            
            if (deleted == false)
                return TResponse.Failure(500, "Failed to delete delivery order.");

            slot.First().IsAvailable = true;
            
            var updatedSlot = await deliverySlotRepository.UpdateAsync(slot.First(), cancellationToken);
            if (updatedSlot == false)
                return TResponse.Failure(500, "Failed to update delivery slot.");

            return TResponse.Successful(deleted, "Delivery order deleted successfully.");
        }

        public async Task<TResponse> GetDeliveryOrdersBatchAsync(Guid tripId, Guid userId, CancellationToken cancellationToken)
        {

            if( tripId != Guid.Empty)
            {
                var trip = await tripRepository.FindAsync([t => t.Id == tripId], cancellationToken);
                if (trip == null || !trip.Any())
                    return TResponse.Failure(404, "Trip not found.");

                var orders = await deliveryOrderRepository.FindWithIncludesAsync([o => o.TripId == tripId], cancellationToken,
                    [o => o.StartLocation, o => o.EndLocation]);

                var orderDtos = mapper.Map<List<GetDeliveryOrderDto>>(orders);
                return TResponse.Successful(orderDtos, "Delivery orders retrieved successfully.");
            }

            if(userId != Guid.Empty)
            {
                var user = await userRepository.FindAsync([u => u.Id == userId], cancellationToken);
                if (user == null || !user.Any())
                    return TResponse.Failure(404, "User not found.");

                var orders = await deliveryOrderRepository.FindWithIncludesAsync([o => o.SenderId == userId], cancellationToken,
                    [o => o.StartLocation, o => o.EndLocation]);
                
                var orderDtos = mapper.Map<List<GetDeliveryOrderDto>>(orders);
                return TResponse.Successful(orderDtos, "Delivery orders retrieved successfully.");
            }

            return TResponse.Failure(400, "Invalid Trip ID or User ID.");
        }

        public async Task<TResponse> SetTripAsCompletedAsync(Guid id, CancellationToken cancellationToken)
        {
            var trip = await tripRepository.FindAsync([t => t.Id == id], cancellationToken);

            if( trip == null || !trip.Any())
                return TResponse.Failure(404, "Trip not found.");

            var orders = await deliveryOrderRepository.FindAsync([o => o.TripId == trip.First().Id], cancellationToken);
            if (orders.Any(o => !o.IsDelivered))
                return TResponse.Failure(400, "Cannot complete trip with undelivered orders.");

            trip.First().IsCompleted = true;
            var updatedTrip = await tripRepository.UpdateAsync(trip.First(), cancellationToken);

            if (updatedTrip == false)
                return TResponse.Failure(500, "Failed to update trip.");

            return TResponse.Successful(updatedTrip, "Trip marked as completed successfully.");
        }

        public async Task<TResponse> SetAsPickedUpAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await deliveryOrderRepository.FindAsync([o => o.Id == id], cancellationToken);

            if (order == null || !order.Any())
                return TResponse.Failure(404, "Delivery order not found.");

            if (order.First().IsPickedUp)
                return TResponse.Failure(400, "Delivery order is already marked as picked up.");

            order.First().IsPickedUp = true;
            
            var updatedOrder = await deliveryOrderRepository.UpdateAsync(order.First(), cancellationToken);
            if (updatedOrder == false)
                return TResponse.Failure(500, "Failed to update delivery order.");

            return TResponse.Successful(updatedOrder, "Delivery order marked as picked up successfully.");
        }

        public async Task<TResponse> SetAsDeliveredAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await deliveryOrderRepository.FindAsync([o => o.Id == id], cancellationToken);
            
            if (order == null || !order.Any())
                return TResponse.Failure(404, "Delivery order not found.");

            if (order.First().IsDelivered)
                return TResponse.Failure(400, "Delivery order is already marked as delivered.");
            
            if (!order.First().IsPickedUp)
                return TResponse.Failure(400, "Delivery order must be picked up before it can be delivered.");
            
            order.First().IsDelivered = true;
            
            var updatedOrder = await deliveryOrderRepository.UpdateAsync(order.First(), cancellationToken);
            if (updatedOrder == false)
                return TResponse.Failure(500, "Failed to update delivery order.");
            
            return TResponse.Successful(updatedOrder, "Delivery order marked as delivered successfully.");
        }
    }
}
