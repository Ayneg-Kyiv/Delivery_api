using Domain.Interfaces.Services;
using Domain.Models.DTOs.Ride.DeliveryOffer;
using Domain.Models.DTOs.Ride.DeliveryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController(IDeliveryRequestService deliveryRequestService) : ControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPut("pickup/{id}")]
        public async Task<IActionResult> SetRequestAsPickedUp([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.SetRequestAsPickedUpAsync(id, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("deliver/{id}")]
        public async Task<IActionResult> SetRequestAsDelivered([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.SetRequestAsDeliveredAsync(id, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDeliveryRequestsBatch(
            [FromQuery] string? cityFrom,
            [FromQuery] string? cityTo,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] bool isPickedUp = false,
            [FromQuery] bool isDelivered = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await deliveryRequestService.GetDeliveryRequestsBatchAsync(
                cityFrom, cityTo, dateFrom, dateTo, isPickedUp, isDelivered, pageNumber, pageSize, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = ("User"))]
        [HttpGet("sender")]
        public async Task<IActionResult> GetDeliveryRequestsBatchBySenderId(
            [FromQuery] Guid? senderId,
            [FromQuery] string? cityFrom,
            [FromQuery] string? cityTo,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] bool isPickedUp = false,
            [FromQuery] bool isDelivered = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await deliveryRequestService.GetDeliveryRequestsBatchBySenderIdAsync(
                senderId, cityFrom, cityTo, dateFrom, dateTo, isPickedUp, isDelivered, pageNumber, pageSize, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("driver")]
        public async Task<IActionResult> GetDeliveryOffersBatchByDriverId(
            [FromQuery] Guid? driverId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await deliveryRequestService.GetDeliveryOffersBatchByDriverIdAsync(
                driverId, pageNumber, pageSize, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = ("User"))]
        [HttpPost("")]
        public async Task<IActionResult> CreateDeliveryRequest([FromBody] CreateDeliveryRequestDto request, CancellationToken cancellationToken)
        {
            var id = HttpContext.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id == null)
                return Unauthorized("Invalid or missing user ID");

            request.SenderId = Guid.Parse(id);

            var result = await deliveryRequestService.CreateDeliveryRequestAsync(request, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
            
        [Authorize(Roles = ("User"))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryRequest([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.DeleteDeliveryRequestAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = ("Driver"))]
        [HttpPost("offer")]
        public async Task<IActionResult> CreateDeliveryOffer([FromBody] CreateDeliveryOfferDto offer, CancellationToken cancellationToken)
        {
            var id = HttpContext.User?.Claims.LastOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id == null)
                return Unauthorized("Invalid or missing user ID");

            offer.DriverId = Guid.Parse(id);

            var result = await deliveryRequestService.CreateDeliveryOfferAsync(offer, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = ("User"))]
        [HttpPut("offer/accept/{offerId}")]
        public async Task<IActionResult> AcceptDeliveryOffer([FromRoute] Guid offerId, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.AcceptDeliveryOfferAsync(offerId, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = ("User"))]
        [HttpDelete("offer/decline/{offerId}")]
        public async Task<IActionResult> DeclineDeliveryOffer([FromRoute] Guid offerId, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.DeclineDeliveryOfferAsync(offerId, HttpContext, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryRequestById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.GetDeliveryRequestByIdAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("offer/{id}")]
        public async Task<IActionResult> GetDeliveryOfferById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await deliveryRequestService.GetDeliveryOfferById(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
