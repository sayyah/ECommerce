using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorGetByNameTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorGetByNameTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "GetByName: Get blogAuthors by Name")]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        foreach (var blogAuthor in expected.Values)
        {
            DbContext.BlogAuthors.Add(blogAuthor);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogAuthorRepository.GetByName(entry.Value.Name, CancellationToken)
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "GetByName: Non existing name")]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        foreach (var blogAuthor in expected.Values)
        {
            DbContext.BlogAuthors.Add(blogAuthor);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogAuthorRepository.GetByName(new Guid().ToString(), CancellationToken)
            );
        }

        // Assert
        actual.Values.Should().AllSatisfy(x => x.Should().BeNull());
    }
}
