using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryAddAllTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAddAllTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "AddAll: Null Argument")]
    public void AddAll_NullArgument_ThrowsException()
    {
        // Act
        Task<int> action() => _categoryRepository.AddAll(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAll: required arguments")]
    public void AddAll_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["required"];

        // Act
        Task<int> actual() => _categoryRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddAll: Add entities to repository")]
    public async void AddAll_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];

        // Act
        int actual = await _categoryRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.Equal(expected.Count, actual);
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
