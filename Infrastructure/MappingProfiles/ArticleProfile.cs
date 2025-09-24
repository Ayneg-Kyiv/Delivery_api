using AutoMapper;
using Domain.Models.DTOs.News;
using Domain.Models.News;

namespace Infrastructure.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, GetArticleDto>();
            CreateMap<CreateArticleDto, Article>();

            CreateMap<ArticleBlock, GetArticleBlockDto>();
            CreateMap<CreateArticleBlockDto, ArticleBlock>();
        }
    }
}
