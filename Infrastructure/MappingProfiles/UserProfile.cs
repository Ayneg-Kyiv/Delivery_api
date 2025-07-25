using AutoMapper;
using Application.DTOs.Identity;
using Domain.Models.Identity;

namespace Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // ApplicationUser → UserDto (для відображення даних)
            CreateMap<ApplicationUser, UserDto>();

            // CreateUserDto → ApplicationUser (для створення)
            CreateMap<CreateUserDto, ApplicationUser>();

            // UpdateUserDto → ApplicationUser (для оновлення профілю)
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
