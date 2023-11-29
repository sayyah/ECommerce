using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentUpdateAsyncTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentUpdateAsyncTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateAsync: Null input")]
    public void UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<BlogComment> actual() => _blogCommentRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update blogComment")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
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
