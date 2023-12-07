using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public async Task AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["required_fields"];

        // Act
       void Actual() => _blogCommentRepository.AddRange(expected.Values, CancellationToken);

        // Assert
        await Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null BlogComment")]
    public async Task AddRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
       void Actual() => _blogCommentRepository.AddRange([ null! ], CancellationToken);

        // Assert
        await Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null argument")]
    public async Task AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
       void Actual() => _blogCommentRepository.AddRange(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add BlogComments all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];

        // Act
        await _blogCommentRepository.AddRange(expected.Values, CancellationToken);

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
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];

        // Act
        await _blogCommentRepository.AddRange(expected.Values, CancellationToken, false);

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
