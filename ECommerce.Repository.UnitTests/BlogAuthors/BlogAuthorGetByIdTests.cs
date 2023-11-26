using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorGetByIdTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorGetByIdTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "GetById: Get blogAuthors by Id")]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        foreach (var blogAuthor in expected.Values)
        {
            DbContext.BlogAuthors.Add(blogAuthor);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogAuthor?> actual = new();
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(entry.Key, _blogAuthorRepository.GetById(entry.Value.Id));
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
