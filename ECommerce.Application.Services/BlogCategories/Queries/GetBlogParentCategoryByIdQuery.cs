using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Results;

namespace ECommerce.Application.Services.BlogCategories.Queries;

public class GetBlogParentCategoryByIdQuery : IQuery<List<BLogCategoryParentResult>>
{
    public int Id { get; set; }
    public bool IsColleague { get; set; }
}

