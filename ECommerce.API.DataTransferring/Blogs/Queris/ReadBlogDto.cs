namespace ECommerce.API.DataTransferObject.Blogs.Queris
{
    public class ReadBlogDto : IBlogDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Title { get; set; }

        public string? Summary { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public DateTime EditDateTime { get; set; } = DateTime.Now;

        public DateTime PublishDateTime { get; set; } = DateTime.Now;

        public string? Url { get; set; }

        public int Like { get; set; }

        public int Dislike { get; set; }

        public int Visit { get; set; }

        public int CommentCount { get; set; }

        //ForeignKey
        public int BlogAuthorId { get; set; }
        //public BlogAuthor? BlogAuthor { get; set; }

        public int BlogCategoryId { get; set; }

        //public ICollection<int> KeywordsId { get; set; }

        public ICollection<int> TagsId { get; set; }
        public List<int>? KeywordsId { get; set; } = new();
        //public List<Keyword>? Keywords { get; set; }
        //public List<BlogTagsDto> Tags { get; set; } = new();
        //public Image? Image { get; set; }
    }
}
