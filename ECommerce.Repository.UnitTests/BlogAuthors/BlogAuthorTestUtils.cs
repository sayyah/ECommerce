using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public partial class BlogAuthorTests : BaseTests
{
    private readonly BlogAuthorRepository _blogAuthorRepository;

    public BlogAuthorTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }
}
