using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "AddRangeAsync: Null Argument")]
    public async Task AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddRangeAsync: required arguments")]
    public async Task AddRangeAsync_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
        Task actual() => _categoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add entities to repository")]
    public async void AddRangeAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];

        // Act
        await _categoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
