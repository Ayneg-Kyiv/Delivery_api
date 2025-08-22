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
    public class ShippingObjectService : IShippingObjectService
    {
        private readonly IBaseRepository<ShippingObject, ShippingDbContext> _repository;
        private readonly IMapper _mapper;

        public ShippingObjectService(IBaseRepository<ShippingObject, ShippingDbContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingObjectDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ShippingObjectDto>>(entities);
        }

        public async Task<ShippingObjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            return entity == null ? null : _mapper.Map<ShippingObjectDto>(entity);
        }

        public async Task<IEnumerable<ShippingObjectDto>> GetByShippingOrderIdAsync(Guid shippingOrderId, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.ShippingOrderId == shippingOrderId, cancellationToken);
            return _mapper.Map<IEnumerable<ShippingObjectDto>>(entities);
        }

        public async Task<ShippingObjectDto> CreateAsync(CreateShippingObjectDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ShippingObject>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastUpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(entity, cancellationToken);

            return _mapper.Map<ShippingObjectDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateShippingObjectDto dto, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            
            if (entity == null)
                return false;

            // Мапуємо тільки ті поля, які не null
            if (dto.Name != null)
                entity.Name = dto.Name;
            if (dto.Description != null)
                entity.Description = dto.Description;
            if (dto.Weight.HasValue)
                entity.Weight = dto.Weight.Value;
            if (dto.Width.HasValue)
                entity.Width = dto.Width.Value;
            if (dto.Height.HasValue)
                entity.Height = dto.Height.Value;
            if (dto.Length.HasValue)
                entity.Length = dto.Length.Value;
            if (dto.ImagePath != null)
                entity.ImagePath = dto.ImagePath;

            entity.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            
            if (entity == null)
                return false;

            await _repository.DeleteAsync(entity, cancellationToken);
            return true;
        }

        public async Task<IEnumerable<ShippingObjectDto>> GetWithPaginationAsync(int pageNumber, int pageSize, Guid? shippingOrderId, CancellationToken cancellationToken)
        {
            var allEntities = await _repository.GetAllAsync(cancellationToken);
            
            if (shippingOrderId.HasValue)
            {
                allEntities = allEntities.Where(x => x.ShippingOrderId == shippingOrderId.Value);
            }

            var paginatedEntities = allEntities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return _mapper.Map<IEnumerable<ShippingObjectDto>>(paginatedEntities);
        }
    }
}
