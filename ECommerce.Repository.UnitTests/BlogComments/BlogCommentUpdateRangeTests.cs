using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void UpdateRange_NullBlogComment_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.UpdateRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void UpdateRange_UpdateEntities_EntitiesChange()
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
        _blogCommentRepository.UpdateRange(expected.Values);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogComments.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
