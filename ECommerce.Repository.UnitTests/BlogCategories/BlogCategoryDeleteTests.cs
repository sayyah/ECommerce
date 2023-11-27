using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryDeleteTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryDeleteTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "Delete: Null blogCategory")]
    public void Delete_NullBlogCategory_ThrowsException()
    {
        // Act
        void action() => _blogCategoryRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Delete: Delete blogCategory from repository")]
    public void Delete_DeleteBlogCategory_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected.Values.ToArray()[0];

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
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToDelete = expected.Values.ToArray()[0];

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
