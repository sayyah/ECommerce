using ECommerce.API.DataTransferObject.Tags;

namespace ECommerce.API.DataTransferObject.Blogs;
public class BlogTagsDto : TagDto
{
    public ICollection<int>? Blogs { get; set; }
}
