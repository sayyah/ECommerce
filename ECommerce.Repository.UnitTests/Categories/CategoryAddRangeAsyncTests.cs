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
        Task Action() => _categoryRepository.AddRange(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "AddRangeAsync: required arguments")]
    public async Task AddRangeAsync_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
       void Actual() => _categoryRepository.AddRange(expected.Values, CancellationToken);

        // Assert
        await Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add entities to repository")]
    public async void AddRangeAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];

        // Act
        await _categoryRepository.AddRange(expected.Values, CancellationToken);

        // Assert
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
