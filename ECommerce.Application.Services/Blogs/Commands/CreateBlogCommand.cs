using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.Blogs.Commands
{
    public class CreateBlogCommand: ICommand<Boolean>
    {
        public string Text { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string Summary { get; set; } = String.Empty;
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime PublishDateTime { get; set; } = DateTime.Now;
        public string Url { get; set; } = String.Empty;
        public int BlogAuthorId { get; set; }
        public int BlogCategoryId { get; set; }
        public List<int>? TagsId { get; set; }
        public List<int>? KeywordsId { get; set; } = new();
    }
}
