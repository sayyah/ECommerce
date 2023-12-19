using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void Delete_NullBlogCategory_ThrowsException()
    {
        // Act
        void Action() => _blogCategoryRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Delete_DeleteBlogCategory_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected["test_1"];

        // Act
        _blogCategoryRepository.Delete(blogCategoryToDelete);

        // Assert
        BlogCategory? actual = DbContext
            .BlogCategories
            .FirstOrDefault(x => x.Id == blogCategoryToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogCategories.Count());
    }

    [Fact(
        DisplayName = "Delete: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public void Delete_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected["test_1"];

        // Act
        _blogCategoryRepository.Delete(blogCategoryToDelete, false);

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
