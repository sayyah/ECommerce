using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.Blogs.Queris
{
    public class GetBlogByTagTextQueryDto
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
        public required string TagText { get; set; }
    }
}
