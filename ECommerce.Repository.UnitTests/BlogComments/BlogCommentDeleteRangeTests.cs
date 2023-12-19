using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void DeleteRange_NullBlogComment_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.DeleteRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void DeleteRange_DeleteBlogComments_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCommentNotToDeleteSetKey = "test_1";
        BlogComment blogCommentNotToDelete = expected[blogCommentNotToDeleteSetKey];
        IEnumerable<BlogComment> blogCommentsToDelete = expected
            .Values
            .Where(x => x.Id != blogCommentNotToDelete.Id);

        // Act
        _blogCommentRepository.DeleteRange(blogCommentsToDelete);

        // Assert
        List<BlogComment?> actual =  [ ];
        foreach (var blogComment in blogCommentsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == blogComment.Id));
        }

        Assert.Equal(1, DbContext.BlogComments.Count());
        foreach (var blogComment in actual)
        {
            Assert.Null(blogComment);
        }
    }

    [Fact(
        DisplayName = "DeleteRange: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public void DeleteRange_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCommentNotToDeleteSetKey = "test_1";
        BlogComment blogCommentNotToDelete = expected[blogCommentNotToDeleteSetKey];
        IEnumerable<BlogComment> blogCommentsToDelete = expected
            .Values
            .Where(x => x.Id != blogCommentNotToDelete.Id);

        // Act
        _blogCommentRepository.DeleteRange(blogCommentsToDelete);

        // Assert
        List<BlogComment?> actual =  [ ];
        foreach (var blogComment in blogCommentsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == blogComment.Id));
        }

        Assert.Equal(expected.Count, DbContext.BlogComments.Count());
        foreach (var blogComment in actual)
        {
            Assert.NotNull(blogComment);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var blogComment in blogCommentsToDelete)
        {
            actual.Add(DbContext.BlogComments.FirstOrDefault(x => x.Id == blogComment.Id));
        }

        Assert.Equal(1, DbContext.BlogComments.Count());
        foreach (var blogComment in actual)
        {
            Assert.Null(blogComment);
        }
    }
}
