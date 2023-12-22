using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task AddAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["required_fields"];

        // Act
        Dictionary<string, Func<Task<BlogComment>>> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                () => _blogCommentRepository.AddAsync(entry.Value, CancellationToken)
            );
        }

        // Assert
        foreach (var action in actual.Values)
        {
            await Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact]
    public async Task AddAsync_NullValue_ThrowsException()
    {
        // Act
        Task Action() => _blogCommentRepository.AddAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        BlogComment expected = TestSets["simple_tests"]["test_1"];

        // Act
        var actual = await _blogCommentRepository.AddAsync(expected, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
