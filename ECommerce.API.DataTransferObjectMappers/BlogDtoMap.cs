using ECommerce.API.DataTransferObject.Blogs;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.API.DataTransferObjectMappers;
public class BlogDtoMap : DtoMap
{
    public BlogDtoMap()
    {
        CreateMap<DeleteBlogDto, DeleteBlogCommand>().ReverseMap();
        CreateMap<EditBlogDto, EditBlogCommand>().ReverseMap();
        CreateMap<CreateBlogDto, CreateBlogCommand>().ReverseMap();
        CreateMap<GetBlogByTagTextQueryDto, GetBlogByTagTextQuery>().ReverseMap();
        CreateMap<GetBlogByUrlQueryDto, GetBlogByUrlQuery>().ReverseMap();
        CreateMap<GetBlogByCategoryQueryDto, GetBlogByCategoryQuery>().ReverseMap();
        CreateMap<GetBlogByIdQueryDto, GetBlogByIdQuery>().ReverseMap();
        CreateMap<GetBlogsQueryDto, GetBlogsQuery>().ReverseMap();
        CreateMap<BlogResult, ReadBlogDto>(); 
        CreateMap<BlogResult, List<ReadBlogDto>>();
    }
}