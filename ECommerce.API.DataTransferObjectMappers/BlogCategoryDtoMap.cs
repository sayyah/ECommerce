using ECommerce.API.DataTransferObject.BlogCategories;
using ECommerce.API.DataTransferObject.BlogCategories.Commands;
using ECommerce.API.DataTransferObject.Blogs.Commands;
using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Application.Services.Blogs.Commands;

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
        CreateMap<CreateBlogCategoryDto, CreateBlogCategoryCommand>().ReverseMap();
        CreateMap<EditBlogCategoryCommand, EditBlogCategoryDto>().ReverseMap();
        CreateMap<DeleteBlogCategoryDto, DeleteBlogCategoryCommand>().ReverseMap();
    }
}
