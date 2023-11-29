using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentDeleteAsyncTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentDeleteAsyncTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteAsync: Null BlogComment")]
    public void DeleteAsync_NullBlogComment_ThrowsException()
    {
        // Act
        Task action() => _blogCommentRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete BlogComment from repository")]
    public async void DeleteAsync_DeleteBlogComment_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected.Values.ToArray()[0];

        // Act
        await _blogCommentRepository.DeleteAsync(blogCommentToDelete, CancellationToken);

        // Assert
        BlogComment? actual = DbContext
            .BlogComments
            .FirstOrDefault(x => x.Id == blogCommentToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogComments.Count());
    }

    [Fact(
        DisplayName = "DeleteAsync: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public async void DeleteAsync_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected.Values.ToArray()[0];

        // Act
        await _blogCommentRepository.DeleteAsync(blogCommentToDelete, CancellationToken, false);

        // Assert
        BlogComment? actual = DbContext
            .BlogComments
            .FirstOrDefault(x => x.Id == blogCommentToDelete.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected.Count, DbContext.BlogComments.Count());

        DbContext.SaveChanges();
        actual = DbContext.BlogComments.FirstOrDefault(x => x.Id == blogCommentToDelete.Id);
        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogComments.Count());
    }
}
