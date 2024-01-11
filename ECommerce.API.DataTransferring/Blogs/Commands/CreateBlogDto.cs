namespace ECommerce.API.DataTransferObject.Blogs.Commands
{
    public class CreateBlogDto : IBlogDto
    {
        public string? Text { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime PublishDateTime { get; set; } = DateTime.Now;
        public string? Url { get; set; }

        //ForeignKey
        public int BlogAuthorId { get; set; }

        public int BlogCategoryId { get; set; }


        public ICollection<int> TagsId { get; set; }
        public List<int>? KeywordsId { get; set; } = new();
    }
}
