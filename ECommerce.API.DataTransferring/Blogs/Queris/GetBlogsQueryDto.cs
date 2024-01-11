using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.Blogs.Queris
{
    public class GetBlogsQueryDto
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
    }
}
