using Domain.Models.DTOs.Order;

namespace Domain.Interfaces.Services
{
    public interface IShippingOfferService
    {
        Task<IEnumerable<ShippingOfferDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShippingOfferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOfferDto>> GetByShippingOrderIdAsync(Guid shippingOrderId, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOfferDto>> GetByCourierIdAsync(Guid courierId, CancellationToken cancellationToken);
        Task<ShippingOfferDto> CreateAsync(CreateShippingOfferDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, UpdateShippingOfferDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ShippingOfferDto>> GetWithPaginationAsync(int pageNumber, int pageSize, Guid? shippingOrderId, Guid? courierId, CancellationToken cancellationToken);
        Task<bool> AcceptOfferAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> RejectOfferAsync(Guid id, CancellationToken cancellationToken);
    }
}
