using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;
using Infrastructure.Contexts;

namespace Application.Services
{
    public class ShippingDestinationService : IShippingDestinationService
    {
        private readonly IBaseRepository<ShippingDestination, ShippingDbContext> _repository;
        private readonly IMapper _mapper;

        public ShippingDestinationService(IBaseRepository<ShippingDestination, ShippingDbContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingDestinationDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync(CancellationToken.None);
            return _mapper.Map<IEnumerable<ShippingDestinationDto>>(entities);
        }

        public async Task<ShippingDestinationDto?> GetByIdAsync(Guid id)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, CancellationToken.None);
            var entity = entities.FirstOrDefault();
            return entity == null ? null : _mapper.Map<ShippingDestinationDto>(entity);
        }

        public async Task<ShippingDestinationDto> AddAsync(CreateShippingDestinationDto createDto)
        {
            var entity = _mapper.Map<ShippingDestination>(createDto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastUpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(entity, CancellationToken.None);
            return _mapper.Map<ShippingDestinationDto>(entity);
        }

        public async Task<ShippingDestinationDto?> UpdateAsync(Guid id, UpdateShippingDestinationDto updateDto)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, CancellationToken.None);
            var existingEntity = entities.FirstOrDefault();
            if (existingEntity == null)
                return null;

            _mapper.Map(updateDto, existingEntity);
            existingEntity.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existingEntity, CancellationToken.None);
            return _mapper.Map<ShippingDestinationDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, CancellationToken.None);
            var existingEntity = entities.FirstOrDefault();
            if (existingEntity == null)
                return false;

            await _repository.DeleteAsync(existingEntity, CancellationToken.None);
            return true;
        }
    }
}
