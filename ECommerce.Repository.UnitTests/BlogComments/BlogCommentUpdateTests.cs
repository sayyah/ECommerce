using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentUpdateTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentUpdateTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "Update: Null input")]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void actual() => _blogCommentRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "Update: Update blogComment")]
    public void Update_UpdateEntity_EntityChanges()
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
        _blogCommentRepository.Update(expectedBlogComment);

        // Assert
        BlogComment? actual = DbContext.BlogComments.Single(p => p.Id == blogCommentToUpdate.Id);
        Assert.Equivalent(expectedBlogComment, actual);
    }
}
