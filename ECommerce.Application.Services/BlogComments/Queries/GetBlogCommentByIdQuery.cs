using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Result;

namespace ECommerce.Application.Services.BlogComments.Queries
{
    public class GetBlogCommentByIdQuery :IQuery<BlogCommentResult>
    {
        public int Id { get; set; }
    }
}
