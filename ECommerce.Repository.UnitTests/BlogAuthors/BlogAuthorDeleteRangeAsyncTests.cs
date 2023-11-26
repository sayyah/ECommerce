using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorDeleteRangeAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorDeleteRangeAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null blogAuthor")]
    public void DeleteRangeAsync_NullBlogAuthor_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.DeleteRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null argument")]
    public void DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogAuthorRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete range of BlogAuthors from repository")]
    public async void DeleteRangeAsync_DeleteBlogAuthors_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogAuthorNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogAuthor blogAuthorNotToDelete = expected[blogAuthorNotToDeleteSetKey];
        IEnumerable<BlogAuthor> blogAuthorsToDelete = expected
            .Values
            .Where(x => x.Id != blogAuthorNotToDelete.Id);

        // Act
        await _blogAuthorRepository.DeleteRangeAsync(blogAuthorsToDelete, CancellationToken);

        // Assert
        List<BlogAuthor?> actual =  [ ];
        foreach (var author in blogAuthorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogAuthors.Count());
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
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string authorNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogAuthor authorNotToDelete = expected[authorNotToDeleteSetKey];
        IEnumerable<BlogAuthor> authorsToDelete = expected
            .Values
            .Where(x => x.Id != authorNotToDelete.Id);

        // Act
        await _blogAuthorRepository.DeleteRangeAsync(authorsToDelete, CancellationToken, false);

        // Assert
        List<BlogAuthor?> actual =  [ ];
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogAuthors.Count());
        foreach (var author in actual)
        {
            Assert.NotNull(author);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogAuthors.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogAuthors.Count());
        foreach (var author in actual)
        {
            Assert.Null(author);
        }
    }
}
