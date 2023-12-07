using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "UpdateRangeAsync: Null BlogComment")]
    public async Task UpdateRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
       void Actual() => _blogCommentRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        await Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public async Task UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
       void Actual() => _blogCommentRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update blogComments")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
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
