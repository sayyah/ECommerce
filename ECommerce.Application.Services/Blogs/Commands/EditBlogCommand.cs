using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Blogs.Commands
{
    public class EditBlogCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime EditDateTime { get; set; } = DateTime.Now;
        public DateTime PublishDateTime { get; set; } = DateTime.Now;
        public string? Url { get; set; }
        public int BlogAuthorId { get; set; }
        public int BlogCategoryId { get; set; }
        public List<int>? TagsId { get; set; }
        public ICollection<BlogComment>? BlogComments { get; set; }
        public List<int>? KeywordsId { get; set; } = new();
    }
}
