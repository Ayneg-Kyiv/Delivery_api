using Domain.Models.DTOs.Order;

namespace Domain.Interfaces.Services
{
    public interface IShippingObjectService
    {
        Task<IEnumerable<ShippingObjectDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShippingObjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingObjectDto>> GetByShippingOrderIdAsync(Guid shippingOrderId, CancellationToken cancellationToken);
        Task<ShippingObjectDto> CreateAsync(CreateShippingObjectDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, UpdateShippingObjectDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingObjectDto>> GetWithPaginationAsync(int pageNumber, int pageSize, Guid? shippingOrderId, CancellationToken cancellationToken);
    }
}
