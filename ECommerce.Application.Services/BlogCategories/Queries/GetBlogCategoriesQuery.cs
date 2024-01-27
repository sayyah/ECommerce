using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Application.Services.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Services.BlogCategories.Results;

namespace ECommerce.Application.Services.BlogCategories.Queries;

public class GetBlogCategoriesQuery : IQuery<PagedList<BlogCategoryResult>>
{
    public PaginationParameters PaginationParameters { get; set; } = new();
}
