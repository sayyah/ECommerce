using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "AddAsync: Null value for required Fields")]
    public async Task AddAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["required_fields"];

        // Act
        Dictionary<string, Func<Task<BlogCategory>>> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                () => _blogCategoryRepository.AddAsync(entry.Value, CancellationToken)
            );
        }

        // Assert
        foreach (var action in actual.Values)
        {
            await Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "AddAsync: Null BlogCategory")]
    public async Task AddAsync_NullValue_ThrowsException()
    {
        // Act
        Task action() => _blogCategoryRepository.AddAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAsync: Add BlogCategory async")]
    public async void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        BlogCategory expected = TestSets["simple_tests"]["test_1"];

        // Act
        var actual = await _blogCategoryRepository.AddAsync(expected, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
