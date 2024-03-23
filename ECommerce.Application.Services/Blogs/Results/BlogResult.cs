using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Blogs.Results;
public class BlogResult
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Title { get; set; }

    public string Summary { get; set; }

    public DateTime CreateDateTime { get; set; } = DateTime.Now;

    public DateTime EditDateTime { get; set; } = DateTime.Now;

    public DateTime PublishDateTime { get; set; } = DateTime.Now;

    public string Url { get; set; }

    public int Like { get; set; }

    public int Dislike { get; set; }

    public int Visit { get; set; }

    public int CommentCount { get; set; }

    //ForeignKey
    public int BlogAuthorId { get; set; }
    public BlogAuthor? BlogAuthor { get; set; }

    public int BlogCategoryId { get; set; }

    //public ICollection<int> KeywordsId { get; set; }

    //public ICollection<int> TagsId { get; set; }
    public List<int>? KeywordsId { get; set; } = new();
    public List<Keyword>? Keywords { get; set; }
    public List<int>? TagsId { get; set; } = new();
    public List<Tag>? Tags { get; set; }
    public Image? Image { get; set; }
}
