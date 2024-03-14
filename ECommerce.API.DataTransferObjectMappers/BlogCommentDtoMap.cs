using ECommerce.API.DataTransferObject.BlogComments.Commands;
using ECommerce.API.DataTransferObject.BlogComments.Queries;
using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Application.Services.BlogComments.Queries;
using ECommerce.Application.Services.BlogComments.Result;

namespace ECommerce.API.DataTransferObjectMappers;

public class BlogCommentDtoMap : DtoMap
{
    public BlogCommentDtoMap()
    {
        CreateMap<DeleteBlogCommentDto, DeleteBlogCommentCommand>().ReverseMap();
        CreateMap<EditBlogCommentDto, EditBlogCommentCommand>().ReverseMap();
        CreateMap<CreateBlogCommentDto, CreateBlogCommentCommand>().ReverseMap();
        CreateMap<GetBlogCommentAllAcceptedQueryDto, GetBlogCommentAllAcceptedQuery>().ReverseMap();
        CreateMap<GetBlogCommentByIdQueryDto, GetBlogCommentByIdQuery>().ReverseMap();
        CreateMap<GetBlogCommentQueryDto, GetBlogCommentQuery>().ReverseMap();
        CreateMap<BlogCommentResult, ReadBlogCommentDto>();
        CreateMap<BlogCommentResult, List<ReadBlogCommentDto>>();
    }
}

