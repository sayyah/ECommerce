using ECommerce.API.DataTransferObject.Blogs;

namespace ECommerce.API.DataTransferObject.BlogComments.Commands
{
    public class DeleteBlogCommentDto : IBlogDto
    {
        public int Id { get; set; }
    }
}
