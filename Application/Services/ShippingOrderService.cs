using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;
using Infrastructure.Contexts;

namespace Application.Services
{
    public class ShippingOrderService : IShippingOrderService
    {
        private readonly IBaseRepository<ShippingOrder, ShippingDbContext> _repository;
        private readonly IMapper _mapper;

        public ShippingOrderService(IBaseRepository<ShippingOrder, ShippingDbContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingOrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ShippingOrderDto>>(entities);
        }

        public async Task<ShippingOrderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            return entity == null ? null : _mapper.Map<ShippingOrderDto>(entity);
        }

        public async Task<IEnumerable<ShippingOrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.CustomerId == customerId, cancellationToken);
            return _mapper.Map<IEnumerable<ShippingOrderDto>>(entities);
        }

        public async Task<ShippingOrderDto> CreateAsync(CreateShippingOrderDto dto, CancellationToken cancellationToken)
        {
            var entity = new ShippingOrder
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                EstimatedCost = dto.EstimatedCost,
                EstimatedDistance = dto.EstimatedDistance,
                EstimatedShippingDate = dto.EstimatedShippingDate,
                EstimatedShippingTime = dto.EstimatedShippingTime,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity, cancellationToken);
            return _mapper.Map<ShippingOrderDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateShippingOrderDto dto, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;

            if (dto.EstimatedCost.HasValue) entity.EstimatedCost = dto.EstimatedCost.Value;
            if (dto.EstimatedDistance.HasValue) entity.EstimatedDistance = dto.EstimatedDistance.Value;
            if (dto.EstimatedShippingDate.HasValue) entity.EstimatedShippingDate = dto.EstimatedShippingDate.Value;
            if (dto.EstimatedShippingTime.HasValue) entity.EstimatedShippingTime = dto.EstimatedShippingTime.Value;

            entity.LastUpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;
            return await _repository.DeleteAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<ShippingOrderDto>> GetWithIncludesAsync(
            Guid? customerId,
            CancellationToken cancellationToken)
        {
            var entities = customerId.HasValue
                ? await _repository.FindWithIncludesAsync(
                    x => x.CustomerId == customerId.Value,
                    cancellationToken,
                    x => x.Offers!,
                    x => x.Objects!)
                : await _repository.FindWithIncludesAsync(
                    x => true,
                    cancellationToken,
                    x => x.Offers!,
                    x => x.Objects!);

            return _mapper.Map<IEnumerable<ShippingOrderDto>>(entities);
        }

        public async Task<IEnumerable<ShippingOrderDto>> GetWithPaginationAsync(
            int pageNumber,
            int pageSize,
            Guid? customerId,
            CancellationToken cancellationToken)
        {
            var entities = customerId.HasValue
                ? await _repository.FindWithIncludesAndPaginationAsync(
                    x => x.CustomerId == customerId.Value,
                    pageNumber,
                    pageSize,
                    cancellationToken,
                    x => x.Offers!,
                    x => x.Objects!)
                : await _repository.FindWithIncludesAndPaginationAsync(
                    x => true,
                    pageNumber,
                    pageSize,
                    cancellationToken,
                    x => x.Offers!,
                    x => x.Objects!);

            return _mapper.Map<IEnumerable<ShippingOrderDto>>(entities);
        }
    }
}
