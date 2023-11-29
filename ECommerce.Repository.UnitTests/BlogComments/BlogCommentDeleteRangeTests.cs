using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentDeleteRangeTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentDeleteRangeTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRange: Null blogComment")]
    public void DeleteRange_NullBlogComment_ThrowsException()
    {
        // Act
        void actual() => _blogCommentRepository.DeleteRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Null argument")]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogCommentRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Delete range of blogComments from repository")]
    public void DeleteRange_DeleteBlogComments_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCommentNotToDeleteSetKey = expected.Keys.ToArray()[0];
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
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string blogCommentNotToDeleteSetKey = expected.Keys.ToArray()[0];
        BlogComment blogCommentNotToDelete = expected[blogCommentNotToDeleteSetKey];
        IEnumerable<BlogComment> blogCommentsToDelete = expected
            .Values
            .Where(x => x.Id != blogCommentNotToDelete.Id);

        // Act
        _blogCommentRepository.DeleteRange(blogCommentsToDelete, false);

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
