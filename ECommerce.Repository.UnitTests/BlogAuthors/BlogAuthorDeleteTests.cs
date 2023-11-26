using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorDeleteTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorDeleteTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "Delete: Null blogAuthor")]
    public void Delete_NullBlogAuthor_ThrowsException()
    {
        // Act
        void action() => _blogAuthorRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Delete: Delete blogAuthor from repository")]
    public void Delete_DeleteBlogAuthor_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToDelete = expected.Values.ToArray()[0];

        // Act
        _blogAuthorRepository.Delete(blogAuthorToDelete);

        // Assert
        BlogAuthor? actual = DbContext
            .BlogAuthors
            .FirstOrDefault(x => x.Id == blogAuthorToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogAuthors.Count());
    }

    [Fact(
        DisplayName = "Delete: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public void Delete_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToDelete = expected.Values.ToArray()[0];

        // Act
        _blogAuthorRepository.Delete(blogAuthorToDelete, false);

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
