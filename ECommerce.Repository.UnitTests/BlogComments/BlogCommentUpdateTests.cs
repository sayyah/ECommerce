using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToUpdate = expected["test_2"];
        BlogComment expectedBlogComment = DbContext
            .BlogComments
            .Single(p => p.Id == blogCommentToUpdate.Id)!;
        expectedBlogComment.Text = Guid.NewGuid().ToString();
        // should other fields be tested too?

        // Act
        _blogCommentRepository.Update(expectedBlogComment);

        // Assert
        BlogComment? actual = DbContext.BlogComments.Single(p => p.Id == blogCommentToUpdate.Id);
        Assert.Equivalent(expectedBlogComment, actual);
    }
}
