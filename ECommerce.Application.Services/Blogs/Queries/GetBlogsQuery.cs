using ECommerce.Application.Services.Objects;
using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities.Helper;

namespace ECommerce.Application.Services.Blogs.Queries;

public class GetBlogsQuery : IQuery<PagedList<BlogViewModel>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}