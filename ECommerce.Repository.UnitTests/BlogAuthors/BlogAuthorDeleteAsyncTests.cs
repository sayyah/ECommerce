using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorDeleteAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorDeleteAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteAsync: Null BlogAuthor")]
    public void DeleteAsync_NullBlogAuthor_ThrowsException()
    {
        // Act
        Task action() => _blogAuthorRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete BlogAuthor from repository")]
    public async void DeleteAsync_DeleteBlogAuthor_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToDelete = expected.Values.ToArray()[0];

        // Act
        await _blogAuthorRepository.DeleteAsync(blogAuthorToDelete, CancellationToken);

        // Assert
        BlogAuthor? actual = DbContext
            .BlogAuthors
            .FirstOrDefault(x => x.Id == blogAuthorToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogAuthors.Count());
    }

    [Fact(
        DisplayName = "DeleteAsync: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public async void DeleteAsync_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToDelete = expected.Values.ToArray()[0];

        // Act
        await _blogAuthorRepository.DeleteAsync(blogAuthorToDelete, CancellationToken, false);

        // Assert
        BlogAuthor? actual = DbContext
            .BlogAuthors
            .FirstOrDefault(x => x.Id == blogAuthorToDelete.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected.Count, DbContext.BlogAuthors.Count());

        DbContext.SaveChanges();
        actual = DbContext.BlogAuthors.FirstOrDefault(x => x.Id == blogAuthorToDelete.Id);
        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogAuthors.Count());
    }
}
