using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Result;
using ECommerce.Application.Services.Objects;

namespace ECommerce.Application.Services.BlogComments.Queries;

public class GetBlogCommentAllAcceptedQuery : IQuery<PagedList<BlogCommentResult>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}