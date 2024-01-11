using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.Application.Services.Blogs.Queries
{
    public class GetBlogByUrlQuery : IQuery<BlogResult>
    {
        public required string BlogUrl { get; set; }
    }
}
