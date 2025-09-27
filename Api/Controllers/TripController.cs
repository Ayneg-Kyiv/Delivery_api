using Domain.Interfaces.Services;
using Domain.Models.DTOs.Ride.DeliveryOrder;
using Domain.Models.DTOs.Ride.DeliverySlot;
using Domain.Models.DTOs.Ride.Location;
using Domain.Models.DTOs.Ride.Trip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(ITripService tripService) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetTrips(
            [FromQuery] string? cityFrom,
            [FromQuery] string? cityTo,
            [FromQuery] decimal? priceFrom,
            [FromQuery] decimal? priceTo,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] double? driverRatingFrom,
            [FromQuery] string? cargoType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool isCompleted = false,
            CancellationToken cancellationToken = default)
        {
            var result = await tripService.GetTripsBatchAsync(
                isCompleted,
                cityFrom, cityTo,
                priceFrom, priceTo,
                dateFrom, dateTo,
                driverRatingFrom,
                cargoType,
                pageNumber,
                pageSize,
                cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("list/with-orders")]
        public async Task<IActionResult> GetTripsWithOrders(
            [FromQuery] Guid? id,
            [FromQuery] string? cityFrom,
            [FromQuery] string? cityTo,
            [FromQuery] decimal? priceFrom,
            [FromQuery] decimal? priceTo,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] double? driverRatingFrom,
            [FromQuery] string? cargoType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool isCompleted = false,
            CancellationToken cancellationToken = default)
        {
            var result = await tripService.GetTripsBatchByDriverIdWithOrdersAsync(
                id,
                isCompleted,
                cityFrom, cityTo,
                priceFrom, priceTo,
                dateFrom, dateTo,
                driverRatingFrom,
                cargoType,
                pageNumber,
                pageSize,
                HttpContext,
                cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("locations/unique")]
        public async Task<IActionResult> GetAllUniqueLocations(CancellationToken cancellationToken)
        {
            var result = await tripService.GetAllUniqueLocations(cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.GetTripByIdAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("with-orders/{id}")]
        public async Task<IActionResult> GetTripByIdWithOrders([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.GetTripByIdWithOrdersAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles ="Driver")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto tripDto, CancellationToken cancellationToken)
        {
            var id = HttpContext.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id == null)
                return Unauthorized("Invalid or missing user ID");

            tripDto.DriverId = Guid.Parse(id);

            var result = await tripService.CreateTripAsync(tripDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTrip([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.DeleteTripAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("location/update")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationDto locationDto, CancellationToken cancellationToken)
        {
            var result = await tripService.UpdateLocationAsync(locationDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("{tripId}/slot/add")]
        public async Task<IActionResult> AddDeliverySlot([FromRoute] Guid tripId, [FromBody] CreateDeliverySlotDto slot, CancellationToken cancellationToken)
        {
            var result = await tripService.AddDeliverySlotAsync(tripId, slot, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{tripId}/orders")]
        public async Task<IActionResult> GetDeliveryOrdersBatch([FromRoute] Guid tripId, [FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            var result = await tripService.GetDeliveryOrdersBatchAsync(tripId, userId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("order/create")]
        public async Task<IActionResult> CreateDeliveryOrder([FromBody] CreateDeliveryOrderDto deliveryOrderDto, CancellationToken cancellationToken)
        {
            var id = HttpContext.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id == null)
                return Unauthorized("Invalid or missing user ID");

            deliveryOrderDto.SenderId = Guid.Parse(id);

            var result = await tripService.CreateDeliveryOrderAsync(deliveryOrderDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("order/delete/{id}")]
        public async Task<IActionResult> DeleteDeliveryOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.DeleteDeliveryOrderAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> SetTripAsCompleted([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.SetTripAsCompletedAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("order/pickup/{id}")]
        public async Task<IActionResult> SetAsPickedUp([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.SetAsPickedUpAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("order/deliver/{id}")]
        public async Task<IActionResult> SetAsDelivered([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.SetAsDeliveredAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetTripDetailsById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.GetTripDetailsByIdAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpPut("start/{tripId}")]
        public async Task<IActionResult> SetTripAsStarted([FromRoute] Guid tripId, CancellationToken cancellationToken)
        {
            var result = await tripService.SetTripAsStartedAsync(tripId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpPut("order/accept/{id}")]
        public async Task<IActionResult> SetAsAccepted([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.SetAsAcceptedAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpPut("order/declined/{id}")]
        public async Task<IActionResult> SetAsDeclined([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.SetAsDeclinedAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("orders/by-sender")]
        public async Task<IActionResult> GetDeliveryOrdersBatchBySenderId(
            [FromQuery] Guid? tripId,
            [FromQuery] Guid? userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await tripService.GetDeliveryOrdersBatchBySenderIdAsync(
                tripId,
                userId,
                pageNumber,
                pageSize,
                HttpContext,
                cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetDeliveryOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.GetDeliveryOrderByIdAsync(id, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }
    }
}