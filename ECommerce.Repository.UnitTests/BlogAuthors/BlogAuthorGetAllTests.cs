using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorGetAllTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorGetAllTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "GetAll: Get all blogAuthors")]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        foreach (var blogAuthor in expected.Values)
        {
            DbContext.BlogAuthors.Add(blogAuthor);
        }
        DbContext.SaveChanges();

        // Act
        var actuals = _blogAuthorRepository.GetAll(CancellationToken).Result;

        // Assert
        actuals.Should().BeEquivalentTo(expected.Values);
    }
}
