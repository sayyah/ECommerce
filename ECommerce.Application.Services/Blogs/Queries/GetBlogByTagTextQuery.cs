using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Application.Services.Objects;

namespace ECommerce.Application.Services.Blogs.Queries
{
    public class GetBlogByTagTextQuery : IQuery<PagedList<BlogResult>>
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
        public required string TagText { get; set; }
    }
}
