using Domain.Models.DTOs.Order;

namespace Domain.Interfaces.Services
{
    public interface IShippingOrderService
    {
        Task<IEnumerable<ShippingOrderDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShippingOrderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<ShippingOrderDto> CreateAsync(CreateShippingOrderDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, UpdateShippingOrderDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOrderDto>> GetWithIncludesAsync(Guid? customerId, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOrderDto>> GetWithPaginationAsync(int pageNumber, int pageSize, Guid? customerId, CancellationToken cancellationToken);
    }
}
