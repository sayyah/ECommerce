using ECommerce.API.DataTransferObject.BlogCategories;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;

namespace ECommerce.API.DataTransferObjectMappers;

public class BlogCategoryDtoMap : DtoMap
{
    public BlogCategoryDtoMap()
    {
        CreateMap<GetBlogCategoriesQueryDto, GetBlogCategoriesQuery>().ReverseMap();
        CreateMap<BlogCategoryResult, ReadBlogCategoryDto>();
        CreateMap<BlogCategoryResult, List<ReadBlogCategoryDto>>();
        CreateMap<GetBlogCategoryByIdQueryDto, GetBlogCategoryByIdQuery>().ReverseMap();
        CreateMap<GetBlogParentCategoryByIdQueryDto, ReadBlogCategoryParentDto>().ReverseMap();
        CreateMap<GetBlogParentCategoryByIdQueryDto, GetBlogParentCategoryByIdQuery>().ReverseMap();
        CreateMap<BLogCategoryParentResult, ReadBlogCategoryParentDto>().ReverseMap();
    }
}
