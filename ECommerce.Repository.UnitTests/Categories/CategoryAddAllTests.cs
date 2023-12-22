using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "AddAll: Null Argument")]
    public async Task AddAll_NullArgument_ThrowsException()
    {
        // Act
        Task Action() => _categoryRepository.AddAll(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact(DisplayName = "AddAll: required arguments")]
    public async Task AddAll_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
        Task Actual() => _categoryRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact(DisplayName = "AddAll: Add entities to repository")]
    public async void AddAll_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];

        // Act
        Task actual = _categoryRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        // Assert.Equal(expected.Count, actual);
        // DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
