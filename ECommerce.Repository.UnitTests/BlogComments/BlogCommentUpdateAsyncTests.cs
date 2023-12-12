using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "UpdateAsync: Null input")]
    public async Task UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<BlogComment> actual() => _blogCommentRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update blogComment")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToUpdate = expected["test_1"];
        BlogComment expectedBlogComment = DbContext
            .BlogComments
            .Single(p => p.Id == blogCommentToUpdate.Id)!;
        expectedBlogComment.Text = Guid.NewGuid().ToString();
        // should other fields be tested too?

        // Act
        await _blogCommentRepository.UpdateAsync(expectedBlogComment, CancellationToken);

        // Assert
        BlogComment? actual = DbContext.BlogComments.Single(p => p.Id == blogCommentToUpdate.Id);
        Assert.Equivalent(expectedBlogComment, actual);
    }
}
