using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async Task AddAsync_NullArgument_ThrowsException()
    {
        // Act
        Task<Category> Action() => _categoryRepository.AddAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task AddAsync_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
        Dictionary<string, Func<Task<Category>>> actual =  [ ];
        foreach (KeyValuePair<string, Category> entry in expected)
        {
            actual.Add(
                entry.Key,
                () => _categoryRepository.AddAsync(entry.Value, CancellationToken)
            );
        }

        // Assert
        foreach (Func<Task<Category>> action in actual.Values)
        {
            await Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact]
    public async void AddAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Category expected = TestSets["simple_tests"]["test_1"];

        // Act
        var actual = await _categoryRepository.AddAsync(expected, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
