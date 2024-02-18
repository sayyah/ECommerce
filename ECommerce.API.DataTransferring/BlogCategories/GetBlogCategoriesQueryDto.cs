using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.BlogCategories;

public class GetBlogCategoriesQueryDto
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}
