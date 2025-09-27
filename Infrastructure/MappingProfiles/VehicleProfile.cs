using AutoMapper;
using Application.DTOs.Vehicles;
using Domain.Models.Vehicles;
using Domain.Models.DTOs.Vehicles;

namespace Application.MappingProfiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            // Domain → DTO
            CreateMap<Vehicle, VehicleDto>();

            // Create DTO → Domain
            CreateMap<CreateVehicleDto, Vehicle>();

            // Update DTO → Domain
            CreateMap<UpdateVehicleDto, Vehicle>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<DriverApplication, GetDriverApplicationDto>();
        }
    }
}
