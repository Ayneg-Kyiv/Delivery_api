using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Domain.Models.DTOs.Reviews;
using Domain.Models.DTOs.Ride.DeliveryOffer;
using Domain.Models.DTOs.Ride.DeliveryRequest;
using Domain.Models.Identity;
using Domain.Models.Reviews;
using Domain.Models.Ride;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Services
{
    public class DeliveryRequestService(
                             IBaseRepository<DeliveryRequest, ShippingDbContext> deliveryRequestRepository,
                             IBaseRepository<DeliveryOffer, ShippingDbContext> deliveryOfferRepository,
                             IBaseRepository<ApplicationUser, IdentityDbContext> userRepository,
                             IBaseRepository<Review, ShippingDbContext> reviewRepository,
                             IMapper mapper) : IDeliveryRequestService
    {
        public async Task<TResponse> GetDeliveryRequestsBatchAsync(
            string? cityFrom,
            string? cityTo,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool isPickedUp,
            bool isDelivered,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            // Build predicates for filtering DeliveryRequests
            List<Expression<Func<DeliveryRequest, bool>>> predicates = [];

            predicates.Add(r => !r.IsAccepted);
            predicates.Add(r => !r.IsPickedUp);
            predicates.Add(r => !r.IsDelivered);

            if (!string.IsNullOrEmpty(cityFrom))
                predicates.Add(r => r.StartLocation.City == cityFrom);
            if(!string.IsNullOrEmpty(cityTo))
                predicates.Add(r => r.EndLocation.City == cityTo);
            if(dateFrom != null)
                predicates.Add(r => r.StartLocation.DateTime == dateFrom);
            if(dateTo != null)
                predicates.Add(r => r.EndLocation.DateTime == dateTo);
            if (isPickedUp)
                predicates.Add(r => r.IsPickedUp);
            if (isDelivered)
                predicates.Add(r => r.IsDelivered);

            // Pagination and includes
            var totalCount = await deliveryRequestRepository.GetTotalCountAsync(predicates, cancellationToken,
                [ d => d.StartLocation, d => d.EndLocation]);
            
            var requests = await deliveryRequestRepository.FindWithIncludesAndPaginationAsync(
                predicates,
                pageNumber,
                pageSize,
                cancellationToken,
                [ d => d.StartLocation, d => d.EndLocation]);

            var requestDtos = mapper.Map<List<GetDeliveryRequestDto>>(requests);

            foreach(var request in requestDtos)
            {
                var user = await userRepository.FindAsync([u => u.Id == request.SenderId], cancellationToken);

                request.Sender = mapper.Map<GetApplicationUserForTripDto>(user.First());
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
                Data = requestDtos
            };

            return TResponse.Successful(paginatedPage, "Delivery requests retrieved successfully.");
        }

        public async Task<TResponse> GetDeliveryRequestsBatchBySenderIdAsync(
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
            CancellationToken cancellationToken)
        {
            // Build predicates for filtering DeliveryRequests
            List<Expression<Func<DeliveryRequest, bool>>> predicates = [];

            if(senderId == null)
                senderId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            predicates.Add(r => r.SenderId == senderId);

            if (!string.IsNullOrEmpty(cityFrom))
                predicates.Add(r => r.StartLocation.City == cityFrom);
            if (!string.IsNullOrEmpty(cityTo))
                predicates.Add(r => r.EndLocation.City == cityTo);
            if (dateFrom != null)
                predicates.Add(r => r.StartLocation.DateTime == dateFrom);
            if (dateTo != null)
                predicates.Add(r => r.EndLocation.DateTime == dateTo);
            if (isPickedUp)
                predicates.Add(r => r.IsPickedUp);
            if (isDelivered)
                predicates.Add(r => r.IsDelivered);

            // Pagination and includes
            var totalCount = await deliveryRequestRepository.GetTotalCountAsync(predicates, cancellationToken,
                [d => d.Offers, d => d.StartLocation, d => d.EndLocation]);

            var requests = await deliveryRequestRepository.FindWithIncludesAndPaginationAsync(
                predicates,
                pageNumber,
                pageSize,
                cancellationToken,
                [d => d.Offers, d => d.StartLocation, d => d.EndLocation]);

            var requestDtos = mapper.Map<List<GetDeliveryRequestDto>>(requests);

            foreach (var request in requestDtos)
            {
                var user = await userRepository.FindAsync([u => u.Id == request.SenderId], cancellationToken);

                request.Sender = mapper.Map<GetApplicationUserForTripDto>(user.First());

                foreach(var offer in request.Offers)
                {
                    var driver = await userRepository.FindAsync([u => u.Id == offer.DriverId], cancellationToken);
                    offer.Driver = mapper.Map<GetApplicationUserForTripDto>(driver.First());
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
                Data = requestDtos
            };

            return TResponse.Successful(paginatedPage, "Delivery requests retrieved successfully.");
        }

        public async Task<TResponse> GetDeliveryRequestByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if( id == Guid.Empty)
                return TResponse.Failure(400, "Delivery request ID must be provided.");

            var request = await deliveryRequestRepository.FindWithIncludesAsync([r => r.Id == id], cancellationToken,
                [d => d.Offers, d => d.StartLocation, d => d.EndLocation]);

            if (request == null || !request.Any())
                return TResponse.Failure(404, "Delivery request not found.");

            var requestDto = mapper.Map<GetDeliveryRequestDto>(request.First());

            requestDto.Sender = mapper.Map<GetApplicationUserForTripDto>(
                (await userRepository.FindAsync([u => u.Id == requestDto.SenderId], cancellationToken)).First());

            requestDto.Sender.Reviews = mapper.Map<List<ReviewDto>>(
                await reviewRepository.FindAsync([r => r.UserId == requestDto.SenderId], cancellationToken));

            return TResponse.Successful(requestDto, "Delivery request details retrieved successfully.");
        }

        public async Task<TResponse> CreateDeliveryRequestAsync(CreateDeliveryRequestDto request, CancellationToken cancellationToken)
        {
            if (request == null)
                return TResponse.Failure(400, "Request data must be provided.");

            var deliveryRequest = mapper.Map<DeliveryRequest>(request);
            var created = await deliveryRequestRepository.AddAsync(deliveryRequest, cancellationToken);

            if (!created)
                return TResponse.Failure(500, "Failed to create delivery request.");

            return TResponse.Successful(deliveryRequest, "Delivery request created successfully.");
        }

        public async Task<TResponse> DeleteDeliveryRequestAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Delivery request ID must be provided.");

            var requests = await deliveryRequestRepository.FindAsync([r => r.Id == id], cancellationToken);

            if (requests == null || !requests.Any())
                return TResponse.Failure(404, "Delivery request not found.");

            var deleted = await deliveryRequestRepository.DeleteAsync(requests.First(), cancellationToken);

            if (!deleted)
                return TResponse.Failure(500, "Failed to delete delivery request.");

            return TResponse.Successful(deleted, "Delivery request deleted successfully.");
        }

        public async Task<TResponse> SetRequestAsPickedUpAsync(Guid id, HttpContext context, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Delivery request ID must be provided.");

            var requests = await deliveryRequestRepository.FindAsync([r => r.Id == id], cancellationToken);
            
            if (requests == null || !requests.Any())
                return TResponse.Failure(404, "Delivery request not found.");

            var request = requests.First();

            if(request.DeliveryOfferID == null && request.IsAccepted == false)
                return TResponse.Failure(400, "No delivery offer has been accepted for this request.");

            var userId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            if(request.SenderId !=  userId)
                return TResponse.Failure(403, "You are not authorized to pick up this delivery request.");

            request.IsPickedUp = true;
            var updated = await deliveryRequestRepository.UpdateAsync(request, cancellationToken);

            if(!updated)
                return TResponse.Failure(500, "Failed to update delivery request.");

            return TResponse.Successful(updated, "Delivery request updated successfully.");
        }

        public async Task<TResponse> SetRequestAsDeliveredAsync(Guid id, HttpContext context, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Delivery request ID must be provided.");

            var requests = await deliveryRequestRepository.FindAsync([r => r.Id == id], cancellationToken);

            if (requests == null || !requests.Any())
                return TResponse.Failure(404, "Delivery request not found.");

            var request = requests.First();

            if (request.DeliveryOfferID == null && !request.IsAccepted )
                return TResponse.Failure(400, "No delivery offer has been accepted for this request.");

            if (!request.IsPickedUp)
                return TResponse.Failure(400, "Request must be picked up first");

            var userId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            if (request.SenderId != userId)
                return TResponse.Failure(403, "You are not authorized to pick up this delivery request.");

            request.IsDelivered = true;
            var updated = await deliveryRequestRepository.UpdateAsync(request, cancellationToken);

            if (!updated)
                return TResponse.Failure(500, "Failed to update delivery request.");

            return TResponse.Successful(updated, "Delivery request updated successfully.");
        }



        public async Task<TResponse> CreateDeliveryOfferAsync(CreateDeliveryOfferDto offer, CancellationToken cancellationToken)
        {
            if (offer == null)
                return TResponse.Failure(400, "Offer data must be provided.");

            var deliveryOffer = mapper.Map<DeliveryOffer>(offer);
            var created = await deliveryOfferRepository.AddAsync(deliveryOffer, cancellationToken);

            if (!created)
                return TResponse.Failure(500, "Failed to create delivery offer.");

            return TResponse.Successful(deliveryOffer, "Delivery offer created successfully.");
        }

        public async Task<TResponse> AcceptDeliveryOfferAsync(Guid offerId, HttpContext context, CancellationToken cancellationToken)
        {
            if (offerId == Guid.Empty)
                return TResponse.Failure(400, "Offer ID must be provided.");

            var offers = await deliveryOfferRepository.FindAsync([o => o.Id == offerId], cancellationToken);

            if (offers == null || !offers.Any())
                return TResponse.Failure(404, "Delivery offer not found.");

            var offer = offers.First();

            // Find the related DeliveryRequest and update its DeliveryOfferID
            var requests = await deliveryRequestRepository.FindAsync([r => r.Id == offer.DeliveryRequestId], cancellationToken);
            if (requests == null || !requests.Any())
                return TResponse.Failure(404, "Related delivery request not found.");

            var request = requests.First();

            var userId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            if (request.SenderId != userId)
                return TResponse.Failure(403, "You are not authorized to accept this delivery offer.");

            request.DeliveryOfferID = offer.Id;
            request.IsAccepted = true;

            var updated = await deliveryRequestRepository.UpdateAsync(request, cancellationToken);

            if (!updated)
                return TResponse.Failure(500, "Failed to accept delivery offer.");
            
            offer.IsAccepted = true;

             updated = await deliveryOfferRepository.UpdateAsync(offer, cancellationToken);

            if (!updated)
                return TResponse.Failure(500, "Failed to accept delivery offer");

            var otherOffers = await deliveryOfferRepository.FindAsync([o => o.DeliveryRequestId == request.Id, o => o.Id != offer.Id], cancellationToken);

            foreach (var off in otherOffers)
            {
                off.IsDeclined = true;

                await deliveryOfferRepository.UpdateAsync(off, cancellationToken);
            }

            return TResponse.Successful(updated, "Delivery offer accepted successfully.");
        }

        public async Task<TResponse> DeclineDeliveryOfferAsync(Guid offerId, HttpContext context, CancellationToken cancellationToken)
        {
            if (offerId == Guid.Empty)
                return TResponse.Failure(400, "Offer ID must be provided.");

            var offers = await deliveryOfferRepository.FindAsync([o => o.Id == offerId], cancellationToken);

            if (offers == null || !offers.Any())
                return TResponse.Failure(404, "Delivery offer not found.");

            var offer = offers.First();


            // Optionally, update the related DeliveryRequest to remove the offer reference
            var requests = await deliveryRequestRepository.FindAsync([r => r.Id == offer.DeliveryRequestId], cancellationToken);
            if (requests != null && requests.Any())
            {
                var request = requests.First();

                var userId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

                if (request.SenderId != userId)
                    return TResponse.Failure(403, "You are not authorized to decline this delivery offer.");

                if (request.DeliveryOfferID == offer.Id)
                {
                    request.DeliveryOfferID = null;
                    await deliveryRequestRepository.UpdateAsync(request, cancellationToken);
                }
            }

            offer.IsDeclined = true;
            
            var updated = await deliveryOfferRepository.UpdateAsync(offer, cancellationToken);

            if (!updated)
                return TResponse.Failure(500, "Failed to decline (delete) delivery offer.");

            return TResponse.Successful(updated, "Delivery offer declined successfully.");
        }


        public async Task<TResponse> GetDeliveryOffersBatchByDriverIdAsync(
            Guid? driverId, 
            int pageNumber, 
            int pageSize, 
            HttpContext context, 
            CancellationToken cancellationToken)
        {
            List<Expression<Func<DeliveryOffer, bool>>> predicates = [];

            if (driverId == null)
                driverId = Guid.Parse(context.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            predicates.Add(o => o.DriverId == driverId);

            var totalCount = await deliveryOfferRepository.GetTotalCountAsync(predicates, cancellationToken,
                [o => o.DeliveryRequest]);

            var offers = await deliveryOfferRepository.FindWithIncludesAndPaginationAsync(
                predicates, 
                pageNumber,
                pageSize, 
                cancellationToken,
                [o => o.DeliveryRequest]);

            var offerDtos = mapper.Map<List<GetDeliveryOfferDto>>(offers);

            foreach (var offer in offerDtos)
            {
                var user = await userRepository.FindAsync([u => u.Id == offer.DriverId], cancellationToken);
                
                offer.Driver = mapper.Map<GetApplicationUserForTripDto>(user.First());
                
                var request = await deliveryRequestRepository.FindWithIncludesAsync([r => r.Id == offer.DeliveryRequestId], cancellationToken,
                    [d => d.StartLocation, d => d.EndLocation]);
                
                if (request != null && request.Any())
                {
                    offer.DeliveryRequest = mapper.Map<GetDeliveryRequestDto>(request.First());

                    offer.DeliveryRequest.Sender = mapper.Map<GetApplicationUserForTripDto>(
                        (await userRepository.FindAsync([u => u.Id == offer.DeliveryRequest.SenderId], cancellationToken)).First());
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
                Data = offerDtos
            };

            return TResponse.Successful(paginatedPage, "Delivery offers retrieved successfully.");
        }

        public async Task<TResponse> GetDeliveryOfferById(Guid id, CancellationToken cancellationToken)
        {
            var offers = await deliveryOfferRepository.FindWithIncludesAsync([o => o.Id == id], cancellationToken,
                [o => o.DeliveryRequest]);

            if (offers == null || !offers.Any())
                return TResponse.Failure(404, "Delivery offer not found.");

            var offerDto = mapper.Map<GetDeliveryOfferDto>(offers.First());

            var user = await userRepository.FindAsync([u => u.Id == offerDto.DriverId], cancellationToken);
            offerDto.Driver = mapper.Map<GetApplicationUserForTripDto>(user.First());

            var request = await deliveryRequestRepository.FindWithIncludesAsync([r => r.Id == offerDto.DeliveryRequestId], cancellationToken,
                [d => d.StartLocation, d => d.EndLocation]);

            if (request == null && !request.Any())
                return TResponse.Failure(404, "Related delivery request not found.");

            offerDto.DeliveryRequest = mapper.Map<GetDeliveryRequestDto>(request.First());
            offerDto.DeliveryRequest.Sender = mapper.Map<GetApplicationUserForTripDto>(
                (await userRepository.FindAsync([u => u.Id == offerDto.DeliveryRequest.SenderId], cancellationToken)).First());

            return TResponse.Successful(offerDto, "Delivery offer details retrieved successfully.");
        }
    }
}
