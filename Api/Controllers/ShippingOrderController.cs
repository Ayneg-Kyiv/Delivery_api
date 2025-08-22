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
    public class ShippingOrderController : ControllerBase
    {
        private readonly IShippingOrderService _shippingOrderService;

        public ShippingOrderController(IShippingOrderService shippingOrderService)
        {
            _shippingOrderService = shippingOrderService;
        }

        /// <summary>
        /// Отримати всі замовлення доставки
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingOrderDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _shippingOrderService.GetAllAsync(cancellationToken);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при отриманні замовлень: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати замовлення доставки за ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShippingOrderDto>> GetById(
            [Required] Guid id, 
            CancellationToken cancellationToken)
        {
            try
            {
                var order = await _shippingOrderService.GetByIdAsync(id, cancellationToken);
                if (order == null)
                {
                    return NotFound($"Замовлення з ID {id} не знайдено");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при отриманні замовлення: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати замовлення доставки за ID клієнта
        /// </summary>
        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IEnumerable<ShippingOrderDto>>> GetByCustomerId(
            [Required] Guid customerId, 
            CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _shippingOrderService.GetByCustomerIdAsync(customerId, cancellationToken);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при отриманні замовлень клієнта: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати замовлення з повними даними (включаючи пов'язані сутності)
        /// </summary>
        [HttpGet("detailed")]
        public async Task<ActionResult<IEnumerable<ShippingOrderDto>>> GetWithIncludes(
            [FromQuery] Guid? customerId, 
            CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _shippingOrderService.GetWithIncludesAsync(customerId, cancellationToken);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при отриманні детальних замовлень: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримати замовлення з пагінацією
        /// </summary>
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ShippingOrderDto>>> GetWithPagination(
            [FromQuery, Required, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Required, Range(1, 100)] int pageSize = 10,
            [FromQuery] Guid? customerId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var orders = await _shippingOrderService.GetWithPaginationAsync(
                    pageNumber, pageSize, customerId, cancellationToken);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при отриманні замовлень з пагінацією: {ex.Message}");
            }
        }

        /// <summary>
        /// Створити нове замовлення доставки
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ShippingOrderDto>> Create(
            [FromBody, Required] CreateShippingOrderDto dto, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdOrder = await _shippingOrderService.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = createdOrder.Id }, 
                    createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при створенні замовлення: {ex.Message}");
            }
        }

        /// <summary>
        /// Оновити замовлення доставки
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(
            [Required] Guid id,
            [FromBody, Required] UpdateShippingOrderDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _shippingOrderService.UpdateAsync(id, dto, cancellationToken);
                if (!success)
                {
                    return NotFound($"Замовлення з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при оновленні замовлення: {ex.Message}");
            }
        }

        /// <summary>
        /// Видалити замовлення доставки
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [Required] Guid id, 
            CancellationToken cancellationToken)
        {
            try
            {
                var success = await _shippingOrderService.DeleteAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound($"Замовлення з ID {id} не знайдено");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при видаленні замовлення: {ex.Message}");
            }
        }
    }
}
