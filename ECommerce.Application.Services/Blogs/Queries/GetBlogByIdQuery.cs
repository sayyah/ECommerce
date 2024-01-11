using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.Application.Services.Blogs.Queries;
public class GetBlogByIdQuery : IQuery<BlogResult>
{
   public int Id { get; set; }
   public bool IsColleague { get; set; }
}
