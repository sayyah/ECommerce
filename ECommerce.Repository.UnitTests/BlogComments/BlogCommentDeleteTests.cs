using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void Delete_NullBlogComment_ThrowsException()
    {
        // Act
        void Action() => _blogCommentRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Delete_DeleteBlogComment_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected["test_1"];

        // Act
        _blogCommentRepository.Delete(blogCommentToDelete);

        // Assert
        BlogComment? actual = DbContext
            .BlogComments
            .FirstOrDefault(x => x.Id == blogCommentToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogComments.Count());
    }

    [Fact(
        DisplayName = "Delete: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public void Delete_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected["test_1"];

        // Act
        _blogCommentRepository.Delete(blogCommentToDelete);

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
