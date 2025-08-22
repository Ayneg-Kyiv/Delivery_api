using Application.DTOs.Identity;
using AutoMapper;
using Domain.Models.Identity;

namespace Application.Mappings.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // Entity → DTO (Read)
            CreateMap<ApplicationUser, ApplicationUserDto>();

            // DTO → Entity (Create)
            CreateMap<CreateApplicationUserDto, ApplicationUser>();

            // DTO → Entity (Update)
            CreateMap<UpdateApplicationUserDto, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
