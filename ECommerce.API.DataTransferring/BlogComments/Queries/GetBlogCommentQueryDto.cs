using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.BlogComments.Queries;

public class GetBlogCommentQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}