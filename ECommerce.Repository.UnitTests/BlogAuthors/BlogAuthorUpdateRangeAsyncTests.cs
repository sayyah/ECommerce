using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorUpdateRangeAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorUpdateRangeAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null BlogAuthor")]
    public void UpdateRangeAsync_NullBlogAuthor_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public void UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update blogAuthors")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            expected[entry.Key] = DbContext.BlogAuthors.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].EnglishName = Guid.NewGuid().ToString();
            expected[entry.Key].Name = Guid.NewGuid().ToString();
            expected[entry.Key].Description = Guid.NewGuid().ToString();
        }

        // Act
        await _blogAuthorRepository.UpdateRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogAuthors.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
