using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryAddAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAddAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "AddAsync: Null Argument")]
    public void AddAsync_NullArgument_ThrowsException()
    {
        // Act
        Task<Category> action() => _categoryRepository.AddAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAsync: required arguments")]
    public void AddAsync_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["required"];

        // Act
        Dictionary<string, Func<Task<Category>>> actual = new();
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
            Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "AddAsync: Add entities to repository")]
    public async void AddAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];

        // Act
        foreach (Category category in expected.Values)
        {
            await _categoryRepository.AddAsync(category, CancellationToken);
        }

        // Assert
        foreach (Category category in expected.Values)
        {
            Category actual = DbContext.Categories.Single(c => c.Id == category.Id);
            actual.Should().BeEquivalentTo(category);
        }
    }
}
