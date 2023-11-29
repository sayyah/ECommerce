using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentAddRangeAsyncTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentAddRangeAsyncTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public void AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["required_fields"];

        // Act
        Task actual() => _blogCommentRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null BlogComment")]
    public void AddRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.AddRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null argument")]
    public void AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCommentRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add BlogComments all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];

        // Act
        await _blogCommentRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRangeAsync: No save")]
    public async void AddRangeAsync_NoSave_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];

        // Act
        await _blogCommentRepository.AddRangeAsync(expected.Values, CancellationToken, false);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
