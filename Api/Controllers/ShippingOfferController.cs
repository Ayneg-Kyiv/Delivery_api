using Domain.Models.DTOs.Order;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Temporarily disabled for testing
    public class ShippingOfferController : ControllerBase
    {
        private readonly IShippingOfferService _shippingOfferService;

        public ShippingOfferController(IShippingOfferService shippingOfferService)
        {
            _shippingOfferService = shippingOfferService;
        }

        /// <summary>
        /// Отримати всі пропозиції доставки
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingOfferDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var offers = await _shippingOfferService.GetAllAsync(cancellationToken);
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати пропозицію доставки за ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShippingOfferDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var offer = await _shippingOfferService.GetByIdAsync(id, cancellationToken);
                if (offer == null)
                {
                    return NotFound($"Пропозицію доставки з ID {id} не знайдено");
                }
                return Ok(offer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати пропозиції доставки за ID замовлення
        /// </summary>
        [HttpGet("by-order/{shippingOrderId:guid}")]
        public async Task<ActionResult<IEnumerable<ShippingOfferDto>>> GetByShippingOrderId(Guid shippingOrderId, CancellationToken cancellationToken)
        {
            try
            {
                var offers = await _shippingOfferService.GetByShippingOrderIdAsync(shippingOrderId, cancellationToken);
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати пропозиції доставки за ID кур'єра
        /// </summary>
        [HttpGet("by-courier/{courierId:guid}")]
        public async Task<ActionResult<IEnumerable<ShippingOfferDto>>> GetByCourierId(Guid courierId, CancellationToken cancellationToken)
        {
            try
            {
                var offers = await _shippingOfferService.GetByCourierIdAsync(courierId, cancellationToken);
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Створити нову пропозицію доставки
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ShippingOfferDto>> Create([FromBody] CreateShippingOfferDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdOffer = await _shippingOfferService.CreateAsync(createDto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = createdOffer.Id }, createdOffer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Оновити пропозицію доставки
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateShippingOfferDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _shippingOfferService.UpdateAsync(id, updateDto, cancellationToken);
                if (!updated)
                {
                    return NotFound($"Пропозицію доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Видалити пропозицію доставки
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _shippingOfferService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                {
                    return NotFound($"Пропозицію доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати пропозиції з пагінацією
        /// </summary>
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ShippingOfferDto>>> GetWithPagination(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? shippingOrderId = null,
            [FromQuery] Guid? courierId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var offers = await _shippingOfferService.GetWithPaginationAsync(pageNumber, pageSize, shippingOrderId, courierId, cancellationToken);
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Прийняти пропозицію доставки
        /// </summary>
        [HttpPatch("{id:guid}/accept")]
        public async Task<ActionResult> AcceptOffer(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var accepted = await _shippingOfferService.AcceptOfferAsync(id, cancellationToken);
                if (!accepted)
                {
                    return NotFound($"Пропозицію доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Відхилити пропозицію доставки
        /// </summary>
        [HttpPatch("{id:guid}/reject")]
        public async Task<ActionResult> RejectOffer(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var rejected = await _shippingOfferService.RejectOfferAsync(id, cancellationToken);
                if (!rejected)
                {
                    return NotFound($"Пропозицію доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}
