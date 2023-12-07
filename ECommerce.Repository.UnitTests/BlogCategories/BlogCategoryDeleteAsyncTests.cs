using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "DeleteAsync: Null BlogCategory")]
    public async Task DeleteAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        Task Action() => _blogCategoryRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete BlogCategory from repository")]
    public async void DeleteAsync_DeleteBlogCategory_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected["test_1"];

        // Act
        await _blogCategoryRepository.DeleteAsync(blogCategoryToDelete, CancellationToken);

        // Assert
        BlogCategory? actual = DbContext
            .BlogCategories
            .FirstOrDefault(x => x.Id == blogCategoryToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogCategories.Count());
    }

    [Fact(
        DisplayName = "DeleteAsync: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public async void DeleteAsync_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected["test_1"];

        // Act
        await _blogCategoryRepository.DeleteAsync(blogCategoryToDelete, CancellationToken, false);

        // Assert
        BlogCategory? actual = DbContext
            .BlogCategories
            .FirstOrDefault(x => x.Id == blogCategoryToDelete.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected.Count, DbContext.BlogCategories.Count());

        DbContext.SaveChanges();
        actual = DbContext.BlogCategories.FirstOrDefault(x => x.Id == blogCategoryToDelete.Id);
        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogCategories.Count());
    }
}
