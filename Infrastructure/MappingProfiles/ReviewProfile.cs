using AutoMapper;
using Domain.Models.DTOs.Reviews;
using Domain.Models.Reviews;

namespace Infrastructure.MappingProfiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Review, ReviewDto>();
        }
    }
}
