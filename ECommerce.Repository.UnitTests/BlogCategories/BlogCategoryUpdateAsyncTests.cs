using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryUpdateAsyncTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryUpdateAsyncTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateAsync: Null input")]
    public void UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<BlogCategory> actual() =>
            _blogCategoryRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update blogCategory")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
        BlogCategory expectedBlogCategory = DbContext
            .BlogCategories
            .Single(p => p.Id == blogCategoryToUpdate.Id)!;
        expectedBlogCategory.Name = Guid.NewGuid().ToString();
        expectedBlogCategory.Description = Guid.NewGuid().ToString();

        // Act
        await _blogCategoryRepository.UpdateAsync(expectedBlogCategory, CancellationToken);

        // Assert
        BlogCategory? actual = DbContext
            .BlogCategories
            .Single(p => p.Id == blogCategoryToUpdate.Id);
        Assert.Equivalent(expectedBlogCategory, actual);
    }
}
