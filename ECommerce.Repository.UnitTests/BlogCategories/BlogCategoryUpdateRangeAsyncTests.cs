using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryUpdateRangeAsyncTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryUpdateRangeAsyncTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null BlogCategory")]
    public void UpdateRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        Task actual() => _blogCategoryRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public void UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCategoryRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update blogCategorys")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            expected[entry.Key] = DbContext.BlogCategories.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].Name = Guid.NewGuid().ToString();
            expected[entry.Key].Description = Guid.NewGuid().ToString();
        }

        // Act
        await _blogCategoryRepository.UpdateRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogCategories.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
