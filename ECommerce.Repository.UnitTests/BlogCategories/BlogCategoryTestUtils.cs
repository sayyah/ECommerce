using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public partial class BlogCategoryTests : BaseTests
{
    private readonly BlogCategoryRepository _blogCategoryRepository;

    public BlogCategoryTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }
}
