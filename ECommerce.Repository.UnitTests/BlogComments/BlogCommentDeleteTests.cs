using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentDeleteTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentDeleteTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "Delete: Null blogComment")]
    public void Delete_NullBlogComment_ThrowsException()
    {
        // Act
        void action() => _blogCommentRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Delete: Delete blogComment from repository")]
    public void Delete_DeleteBlogComment_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected.Values.ToArray()[0];

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
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToDelete = expected.Values.ToArray()[0];

        // Act
        _blogCommentRepository.Delete(blogCommentToDelete, false);

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
