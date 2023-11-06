using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryAddRangeAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAddRangeAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "AddRangeAsync: Null Argument")]
    public void AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddRangeAsync: required arguments")]
    public void AddRangeAsync_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["required"];

        // Act
        Task actual() => _categoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add entities to repository")]
    public async void AddRangeAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];

        // Act
        await _categoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
