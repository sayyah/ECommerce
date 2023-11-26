using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorGetByIdAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorGetByIdAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "GetByIdAsync: Get blogAuthors by Id")]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        foreach (var blogAuthor in expected.Values)
        {
            DbContext.BlogAuthors.Add(blogAuthor);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogAuthor> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogAuthorRepository.GetByIdAsync(CancellationToken, entry.Value.Id)
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
