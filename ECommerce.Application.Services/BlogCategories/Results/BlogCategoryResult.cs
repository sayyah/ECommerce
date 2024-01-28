using ECommerce.Domain.Entities;
using Newtonsoft.Json;

namespace ECommerce.Application.Services.BlogCategories.Results;

public class BlogCategoryResult
{
    public int Id { get; set; }
    public int? CreatorUserId { get; set; }
    public int? EditorUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? Name { get; set; }

    public int? Depth { get; set; } = 0;

    public string? Description { get; set; }

    //ForeignKey
    public int? ParentId { get; set; }
    public BlogCategory? Parent { get; set; }

    [JsonIgnore] public ICollection<BlogCategory>? BlogCategories { get; set; } = new List<BlogCategory>();

    public ICollection<Blog>? Blogs { get; set; }
}

