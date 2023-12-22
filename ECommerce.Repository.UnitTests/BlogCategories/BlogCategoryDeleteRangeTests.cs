using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void DeleteRange_NullBlogCategory_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.DeleteRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void DeleteRange_DeleteBlogCategories_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCategoryNotToDeleteSetKey = "test_1";
        BlogCategory blogCategoryNotToDelete = expected[blogCategoryNotToDeleteSetKey];
        IEnumerable<BlogCategory> blogCategorysToDelete = expected
            .Values
            .Where(x => x.Id != blogCategoryNotToDelete.Id);

        // Act
        _blogCategoryRepository.DeleteRange(blogCategorysToDelete);

        // Assert
        List<BlogCategory?> actual =  [ ];
        foreach (var blogCategory in blogCategorysToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == blogCategory.Id));
        }

        Assert.Equal(1, DbContext.BlogCategories.Count());
        foreach (var blogCategory in actual)
        {
            Assert.Null(blogCategory);
        }
    }

    [Fact(
        DisplayName = "DeleteRange: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public void DeleteRange_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCategoryNotToDeleteSetKey = "test_1";
        BlogCategory blogCategoryNotToDelete = expected[blogCategoryNotToDeleteSetKey];
        IEnumerable<BlogCategory> blogCategorysToDelete = expected
            .Values
            .Where(x => x.Id != blogCategoryNotToDelete.Id);

        // Act
        _blogCategoryRepository.DeleteRange(blogCategorysToDelete);

        // Assert
        List<BlogCategory?> actual =  [ ];
        foreach (var blogCategory in blogCategorysToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == blogCategory.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogCategories.Count());
        foreach (var blogCategory in actual)
        {
            Assert.NotNull(blogCategory);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var blogCategory in blogCategorysToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == blogCategory.Id));
        }

        Assert.Equal(1, DbContext.BlogCategories.Count());
        foreach (var blogCategory in actual)
        {
            Assert.Null(blogCategory);
        }
    }
}
