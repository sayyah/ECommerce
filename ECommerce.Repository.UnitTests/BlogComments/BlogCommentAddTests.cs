using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "Add: Null value for required Fields")]
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

    [Fact(DisplayName = "Add: Null BlogComment value")]
    public void Add_NullValue_ThrowsException()
    {
        // Act
        Dictionary<string, Action> actual =  [ ];
        void action() => _blogCommentRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Add: Add BlogComment")]
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
