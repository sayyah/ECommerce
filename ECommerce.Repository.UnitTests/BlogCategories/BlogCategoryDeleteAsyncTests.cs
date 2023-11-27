using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryDeleteAsyncTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryDeleteAsyncTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteAsync: Null BlogCategory")]
    public void DeleteAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        Task action() => _blogCategoryRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete BlogCategory from repository")]
    public async void DeleteAsync_DeleteBlogCategory_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected.Values.ToArray()[0];

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
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected.Values.ToArray()[0];

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
