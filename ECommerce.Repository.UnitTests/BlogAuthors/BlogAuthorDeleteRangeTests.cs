using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorDeleteRangeTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorDeleteRangeTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRange: Null blogAuthor")]
    public void DeleteRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.DeleteRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Null argument")]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Delete range of blogAuthors from repository")]
    public void DeleteRange_DeleteBlogAuthors_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogAuthorNotToDeleteSetKey = expected.Keys.ToArray()[0];
        BlogAuthor blogAuthorNotToDelete = expected[blogAuthorNotToDeleteSetKey];
        IEnumerable<BlogAuthor> blogAuthorsToDelete = expected
            .Values
            .Where(x => x.Id != blogAuthorNotToDelete.Id);

        // Act
        _blogAuthorRepository.DeleteRange(blogAuthorsToDelete);

        // Assert
        List<BlogAuthor?> actual =  [ ];
        foreach (var blogAuthor in blogAuthorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == blogAuthor.Id));
        }

        Assert.Equal(1, DbContext.BlogAuthors.Count());
        foreach (var blogAuthor in actual)
        {
            Assert.Null(blogAuthor);
        }
    }

    [Fact(
        DisplayName = "DeleteRange: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public void DeleteRange_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogAuthorNotToDeleteSetKey = expected.Keys.ToArray()[0];
        BlogAuthor blogAuthorNotToDelete = expected[blogAuthorNotToDeleteSetKey];
        IEnumerable<BlogAuthor> blogAuthorsToDelete = expected
            .Values
            .Where(x => x.Id != blogAuthorNotToDelete.Id);

        // Act
        _blogAuthorRepository.DeleteRange(blogAuthorsToDelete, false);

        // Assert
        List<BlogAuthor?> actual =  [ ];
        foreach (var blogAuthor in blogAuthorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == blogAuthor.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogAuthors.Count());
        foreach (var blogAuthor in actual)
        {
            Assert.NotNull(blogAuthor);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var blogAuthor in blogAuthorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == blogAuthor.Id));
        }

        Assert.Equal(1, DbContext.BlogAuthors.Count());
        foreach (var blogAuthor in actual)
        {
            Assert.Null(blogAuthor);
        }
    }
}
