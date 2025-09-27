using Domain.Interfaces.Services;
using Domain.Models.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingDestinationController : ControllerBase
    {
        private readonly IShippingDestinationService _service;

        public ShippingDestinationController(IShippingDestinationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingDestinationDto>>> GetAll()
        {
            var destinations = await _service.GetAllAsync();
            return Ok(destinations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingDestinationDto>> GetById([FromRoute]Guid id)
        {
            var destination = await _service.GetByIdAsync(id);
            
            if (destination == null)
                return NotFound();
            
            return Ok(destination);
        }

        [HttpPost]
        public async Task<ActionResult<ShippingDestinationDto>> Create([FromBody]CreateShippingDestinationDto createDto)
        {
            var destination = await _service.AddAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = destination.Id }, destination);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShippingDestinationDto>> Update([FromRoute]Guid id, [FromBody]UpdateShippingDestinationDto updateDto)
        {
            var destination = await _service.UpdateAsync(id, updateDto);
            
            if (destination == null)
                return NotFound();
            
            return Ok(destination);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            var success = await _service.DeleteAsync(id);
            
            if (!success)
                return NotFound();
            
            return NoContent();
        }
    }
}
