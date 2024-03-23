using ECommerce.API.DataTransferObject.Keywords;

namespace ECommerce.API.DataTransferObject.Blogs
{
    public class BlogKeywordsDto : KeywordDto
    {
        public ICollection<int>? Blogs { get; set; }
    }
}
