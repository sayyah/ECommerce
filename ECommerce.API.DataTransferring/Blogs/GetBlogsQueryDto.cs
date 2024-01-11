using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferring.Blogs
{
    public class GetBlogsQueryDto
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
    }
}
