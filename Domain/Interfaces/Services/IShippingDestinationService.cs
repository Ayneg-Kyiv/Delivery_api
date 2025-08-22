using Domain.Models.DTOs.Order;

namespace Domain.Interfaces.Services
{
    public interface IShippingDestinationService
    {
        Task<IEnumerable<ShippingDestinationDto>> GetAllAsync();
        Task<ShippingDestinationDto?> GetByIdAsync(Guid id);
        Task<ShippingDestinationDto> AddAsync(CreateShippingDestinationDto createDto);
        Task<ShippingDestinationDto?> UpdateAsync(Guid id, UpdateShippingDestinationDto updateDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
