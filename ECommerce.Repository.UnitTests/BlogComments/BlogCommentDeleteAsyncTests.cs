using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "DeleteAsync: Null BlogComment")]
    public async Task DeleteAsync_NullBlogComment_ThrowsException()
    {
        // Act
        Task Action() => _blogCommentRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete BlogComment from repository")]
    public async void DeleteAsync_DeleteBlogComment_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected["test_1"];

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
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected["test_1"];

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
