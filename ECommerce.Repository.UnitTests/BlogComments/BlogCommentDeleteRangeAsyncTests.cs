using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentDeleteRangeAsyncTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentDeleteRangeAsyncTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null blogComment")]
    public void DeleteRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.DeleteRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null argument")]
    public void DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete range of BlogComments from repository")]
    public async void DeleteRangeAsync_DeleteBlogComments_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCommentNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogComment blogCommentNotToDelete = expected[blogCommentNotToDeleteSetKey];
        IEnumerable<BlogComment> blogCommentsToDelete = expected
            .Values
            .Where(x => x.Id != blogCommentNotToDelete.Id);

        // Act
        await _blogCommentRepository.DeleteRangeAsync(blogCommentsToDelete, CancellationToken);

        // Assert
        List<BlogComment?> actual =  [ ];
        foreach (var author in blogCommentsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogComments.Count());
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
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string authorNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        BlogComment authorNotToDelete = expected[authorNotToDeleteSetKey];
        IEnumerable<BlogComment> authorsToDelete = expected
            .Values
            .Where(x => x.Id != authorNotToDelete.Id);

        // Act
        await _blogCommentRepository.DeleteRangeAsync(authorsToDelete, CancellationToken, false);

        // Assert
        List<BlogComment?> actual =  [ ];
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogComments.Count());
        foreach (var author in actual)
        {
            Assert.NotNull(author);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var author in authorsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == author.Id));
        }

        Assert.Equal(1, DbContext.BlogComments.Count());
        foreach (var author in actual)
        {
            Assert.Null(author);
        }
    }
}
