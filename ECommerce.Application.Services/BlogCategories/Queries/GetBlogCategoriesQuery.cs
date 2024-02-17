using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Application.Services.Objects;

namespace ECommerce.Application.Services.BlogCategories.Queries;

public class GetBlogCategoriesQuery : IQuery<PagedList<BlogCategoryResult>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}
