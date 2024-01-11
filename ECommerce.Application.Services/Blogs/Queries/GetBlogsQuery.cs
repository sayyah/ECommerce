using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Application.Services.Objects;

namespace ECommerce.Application.Services.Blogs.Queries;

public class GetBlogsQuery : IQuery<PagedList<BlogResult>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}