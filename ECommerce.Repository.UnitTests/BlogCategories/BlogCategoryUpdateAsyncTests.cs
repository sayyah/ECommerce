using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "UpdateAsync: Null input")]
    public async Task UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<BlogCategory> actual() =>
            _blogCategoryRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update blogCategory")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
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
