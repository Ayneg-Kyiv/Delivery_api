using Domain.Models.DTOs.Order;
using Domain.Models.Orders;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Contexts;

namespace Application.Services
{
    public class ShippingOfferService : IShippingOfferService
    {
        private readonly IBaseRepository<ShippingOffer, ShippingDbContext> _repository;
        private readonly IMapper _mapper;

        public ShippingOfferService(IBaseRepository<ShippingOffer, ShippingDbContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingOfferDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ShippingOfferDto>>(entities);
        }

        public async Task<ShippingOfferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            return entity == null ? null : _mapper.Map<ShippingOfferDto>(entity);
        }

        public async Task<IEnumerable<ShippingOfferDto>> GetByShippingOrderIdAsync(Guid shippingOrderId, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.ShippingOrderId == shippingOrderId, cancellationToken);
            return _mapper.Map<IEnumerable<ShippingOfferDto>>(entities);
        }

        public async Task<IEnumerable<ShippingOfferDto>> GetByCourierIdAsync(Guid courierId, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.CourierId == courierId, cancellationToken);
            return _mapper.Map<IEnumerable<ShippingOfferDto>>(entities);
        }

        public async Task<ShippingOfferDto> CreateAsync(CreateShippingOfferDto dto, CancellationToken cancellationToken)
        {
            var entity = new ShippingOffer
            {
                Id = Guid.NewGuid(),
                ShippingOrderId = dto.ShippingOrderId,
                CourierId = dto.CourierId,
                OfferedPrice = dto.OfferedPrice,
                OfferedDate = dto.OfferedDate,
                OfferedTime = dto.OfferedTime,
                IsAccepted = false, // Default value for new offers
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity, cancellationToken);
            return _mapper.Map<ShippingOfferDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateShippingOfferDto dto, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;

            if (dto.OfferedPrice.HasValue) entity.OfferedPrice = dto.OfferedPrice.Value;
            if (dto.OfferedDate.HasValue) entity.OfferedDate = dto.OfferedDate.Value;
            if (dto.OfferedTime.HasValue) entity.OfferedTime = dto.OfferedTime.Value;
            if (dto.IsAccepted.HasValue) entity.IsAccepted = dto.IsAccepted.Value;
            if (dto.Comment != null) entity.Comment = dto.Comment;

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

        public async Task<IEnumerable<ShippingOfferDto>> GetWithPaginationAsync(
            int pageNumber, 
            int pageSize, 
            Guid? shippingOrderId, 
            Guid? courierId, 
            CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(
                x => (!shippingOrderId.HasValue || x.ShippingOrderId == shippingOrderId.Value) &&
                     (!courierId.HasValue || x.CourierId == courierId.Value),
                cancellationToken);

            // Simulate pagination
            var paginatedEntities = entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return _mapper.Map<IEnumerable<ShippingOfferDto>>(paginatedEntities);
        }

        public async Task<bool> AcceptOfferAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;

            entity.IsAccepted = true;
            entity.LastUpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> RejectOfferAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;

            entity.IsAccepted = false;
            entity.LastUpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity, cancellationToken);
        }
    }
}
