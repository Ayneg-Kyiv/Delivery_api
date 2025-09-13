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
    public class ShippingObjectController : ControllerBase
    {
        private readonly IShippingObjectService _shippingObjectService;

        public ShippingObjectController(IShippingObjectService shippingObjectService)
        {
            _shippingObjectService = shippingObjectService;
        }

        /// <summary>
        /// Отримати всі об'єкти доставки
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingObjectDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var objects = await _shippingObjectService.GetAllAsync(cancellationToken);
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати об'єкт доставки за ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShippingObjectDto>> GetById([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var obj = await _shippingObjectService.GetByIdAsync(id, cancellationToken);
                if (obj == null)
                {
                    return NotFound($"Об'єкт доставки з ID {id} не знайдено");
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати об'єкти доставки за ID замовлення
        /// </summary>
        [HttpGet("by-order/{shippingOrderId:guid}")]
        public async Task<ActionResult<IEnumerable<ShippingObjectDto>>> GetByShippingOrderId([FromRoute]Guid shippingOrderId, CancellationToken cancellationToken)
        {
            try
            {
                var objects = await _shippingObjectService.GetByShippingOrderIdAsync(shippingOrderId, cancellationToken);
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Створити новий об'єкт доставки
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ShippingObjectDto>> Create([FromBody] CreateShippingObjectDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdObject = await _shippingObjectService.CreateAsync(createDto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = createdObject.Id }, createdObject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Оновити об'єкт доставки
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateShippingObjectDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _shippingObjectService.UpdateAsync(id, updateDto, cancellationToken);
                if (!updated)
                {
                    return NotFound($"Об'єкт доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Видалити об'єкт доставки
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _shippingObjectService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                {
                    return NotFound($"Об'єкт доставки з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати об'єкти з пагінацією
        /// </summary>
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ShippingObjectDto>>> GetWithPagination(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? shippingOrderId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest("Номер сторінки повинен бути більше 0");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Розмір сторінки повинен бути від 1 до 100");
                }

                var objects = await _shippingObjectService.GetWithPaginationAsync(pageNumber, pageSize, shippingOrderId, cancellationToken);
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}
