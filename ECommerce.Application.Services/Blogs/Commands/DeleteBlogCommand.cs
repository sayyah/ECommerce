using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.Application.Services.Blogs.Commands
{
    public class DeleteBlogCommand : ICommand<BlogResult>
    {
        public int Id { get; set; }
    }
}
