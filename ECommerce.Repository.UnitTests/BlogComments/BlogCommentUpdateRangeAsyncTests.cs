using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentUpdateRangeAsyncTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentUpdateRangeAsyncTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null BlogComment")]
    public void UpdateRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public void UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update blogComments")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            expected[entry.Key] = DbContext.BlogComments.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].Text = Guid.NewGuid().ToString();
            // should other fields be tested too?
        }

        // Act
        await _blogCommentRepository.UpdateRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogComments.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
