using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryDeleteRangeAsyncTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryDeleteRangeAsyncTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null blogCategory")]
    public void DeleteRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        Task actual() => _blogCategoryRepository.DeleteRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null argument")]
    public void DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCategoryRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete range of BlogCategories from repository")]
    public async void DeleteRangeAsync_DeleteBlogCategories_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCategoryNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogCategory blogCategoryNotToDelete = expected[blogCategoryNotToDeleteSetKey];
        IEnumerable<BlogCategory> blogCategorysToDelete = expected
            .Values
            .Where(x => x.Id != blogCategoryNotToDelete.Id);

        // Act
        await _blogCategoryRepository.DeleteRangeAsync(blogCategorysToDelete, CancellationToken);

        // Assert
        List<BlogCategory?> actual =  [ ];
        foreach (var author in blogCategorysToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogCategories.Count());
        foreach (var author in actual)
        {
            Assert.Null(author);
        }
    }

    [Fact(
        DisplayName = "DeleteRangeAsync: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public async void DeleteRangeAsync_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string authorNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogCategory authorNotToDelete = expected[authorNotToDeleteSetKey];
        IEnumerable<BlogCategory> authorsToDelete = expected
            .Values
            .Where(x => x.Id != authorNotToDelete.Id);

        // Act
        await _blogCategoryRepository.DeleteRangeAsync(authorsToDelete, CancellationToken, false);

        // Assert
        List<BlogCategory?> actual =  [ ];
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogCategories.Count());
        foreach (var author in actual)
        {
            Assert.NotNull(author);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogCategories.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogCategories.Count());
        foreach (var author in actual)
        {
            Assert.Null(author);
        }
    }
}