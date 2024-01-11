using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.Blogs.Commands
{
    public class DeleteBlogCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}
