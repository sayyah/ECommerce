using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.DataTransferObjectMappers;
public class BlogCommentDtoMap : DtoMap
{
    public BlogCommentDtoMap()
    {
        CreateMap<CreateBlogCommentCommand, BlogComment>();
        CreateMap<EditBlogCommentCommand, BlogComment>();
    }
}