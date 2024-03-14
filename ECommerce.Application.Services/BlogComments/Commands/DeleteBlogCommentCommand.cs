using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.BlogComments.Commands
{
    public class DeleteBlogCommentCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}
