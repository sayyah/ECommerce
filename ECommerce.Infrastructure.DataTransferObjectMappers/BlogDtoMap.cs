using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.DataTransferObjectMappers;
public class BlogDtoMap : DtoMap
{
    public BlogDtoMap()
    {
        CreateMap<CreateBlogCommand, Blog>();
        CreateMap<EditBlogCommand, Blog>();
    }
}