using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorAddRangeAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorAddRangeAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public void AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["required_fields"];

        // Act
        Task actual() => _blogAuthorRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null BlogAuthor")]
    public void AddRangeAsync_NullBlogAuthor_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.AddRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null argument")]
    public void AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add BlogAuthors all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        await _blogAuthorRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogAuthors.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRangeAsync: No save")]
    public async void AddRangeAsync_NoSave_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        await _blogAuthorRepository.AddRangeAsync(expected.Values, CancellationToken, false);

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogAuthors.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogAuthors.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
