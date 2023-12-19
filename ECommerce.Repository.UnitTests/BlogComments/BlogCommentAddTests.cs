using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void Add_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["required_fields"];

        // Act
        Dictionary<string, Action> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(entry.Key, () => _blogCommentRepository.Add(entry.Value));
        }

        // Assert
        foreach (var action in actual.Values)
        {
            Assert.Throws<DbUpdateException>(action);
        }
    }

    [Fact]
    public void Add_NullValue_ThrowsException()
    {
        // Act
        Dictionary<string, Action> actual =  [ ];
        void Action() => _blogCommentRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        BlogComment expected = TestSets["simple_tests"]["test_1"];

        // Act
        _blogCommentRepository.Add(expected);

        // Assert
        var actual = DbContext.BlogComments.FirstOrDefault(x => x.Id == expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }
}
