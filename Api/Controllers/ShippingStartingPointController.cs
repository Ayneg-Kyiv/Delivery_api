// filepath: c:\Users\stas_\Desktop\STEP\PROJ\Delivery_api\Api\Controllers\ShippingStartingPointController.cs
using Domain.Models.DTOs.Order;
using Application.Services;
using Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingStartingPointController : ControllerBase
    {
        private readonly ShippingStartingPointService _service;

        public ShippingStartingPointController(ShippingStartingPointService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(TResponse.Successful(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            if (result == null)
                return NotFound(TResponse.Failure(404, "Shipping starting point not found."));
            return Ok(TResponse.Successful(result));
        }

        [HttpGet("by-order/{orderId:guid}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken cancellationToken)
        {
            var result = await _service.GetByOrderIdAsync(orderId, cancellationToken);
            return Ok(TResponse.Successful(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShippingStartingPointDto dto, CancellationToken cancellationToken)
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return Ok(TResponse.Successful(created, "Shipping starting point created."));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShippingStartingPointDto dto, CancellationToken cancellationToken)
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            if (!updated)
                return NotFound(TResponse.Failure(404, "Shipping starting point not found."));
            return Ok(TResponse.Successful(null, "Shipping starting point updated."));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _service.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound(TResponse.Failure(404, "Shipping starting point not found."));
            return Ok(TResponse.Successful(null, "Shipping starting point deleted."));
        }
    }
}