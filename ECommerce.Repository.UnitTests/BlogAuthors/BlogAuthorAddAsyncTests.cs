using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorAddAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorAddAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "AddAsync: Null value for required Fields")]
    public void AddAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["required_fields"];

        // Act
        Dictionary<string, Func<Task<BlogAuthor>>> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                () => _blogAuthorRepository.AddAsync(entry.Value, CancellationToken)
            );
        }

        // Assert
        foreach (var action in actual.Values)
        {
            Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "AddAsync: Null BlogAuthor")]
    public void AddAsync_NullValue_ThrowsException()
    {
        // Act
        Task action() => _blogAuthorRepository.AddAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAsync: Add BlogAuthor async")]
    public void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        Dictionary<string, BlogAuthor> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                _blogAuthorRepository.AddAsync(entry.Value, CancellationToken).Result
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
