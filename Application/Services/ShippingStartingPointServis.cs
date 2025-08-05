// filepath: c:\Users\stas_\Desktop\STEP\PROJ\Delivery_api\Application\Services\ShippingStartingPointServis.cs
using Domain.Models.DTOs.Order;
using Domain.Models.Orders;
using Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Application.Services
{
    public class ShippingStartingPointService
    {
        private readonly IBaseRepository<ShippingStartingPoint> _repository;
        private readonly IMapper _mapper;

        public ShippingStartingPointService(IBaseRepository<ShippingStartingPoint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingStartingPointDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ShippingStartingPointDto>>(entities);
        }

        public async Task<ShippingStartingPointDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            return entity == null ? null : _mapper.Map<ShippingStartingPointDto>(entity);
        }

        public async Task<IEnumerable<ShippingStartingPointDto>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.ShippingOrderId == orderId, cancellationToken);
            return _mapper.Map<IEnumerable<ShippingStartingPointDto>>(entities);
        }

        public async Task<ShippingStartingPointDto> CreateAsync(CreateShippingStartingPointDto dto, CancellationToken cancellationToken)
        {
            var entity = new ShippingStartingPoint
            {
                Id = Guid.NewGuid(),
                ShippingOrderId = dto.ShippingOrderId,
                Country = dto.Country,
                City = dto.City,
                District = dto.District,
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                PhoneNumber = dto.PhoneNumber,
                AdditionalInfo = dto.AdditionalInfo,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity, cancellationToken);
            return _mapper.Map<ShippingStartingPointDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateShippingStartingPointDto dto, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(x => x.Id == id, cancellationToken);
            var entity = entities.FirstOrDefault();
            if (entity == null) return false;

            if (dto.Country != null) entity.Country = dto.Country;
            if (dto.City != null) entity.City = dto.City;
            if (dto.District != null) entity.District = dto.District;
            if (dto.Street != null) entity.Street = dto.Street;
            if (dto.HouseNumber != null) entity.HouseNumber = dto.HouseNumber;
            if (dto.PhoneNumber != null) entity.PhoneNumber = dto.PhoneNumber;
            if (dto.AdditionalInfo != null) entity.AdditionalInfo = dto.AdditionalInfo;

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
    }
}