using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryAddTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAddTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "Add: Null Argument")]
    public void Add_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Add: required arguments")]
    public void Add_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["required"];

        // Act
        Dictionary<string, Action> actual = new();
        foreach (KeyValuePair<string, Category> entry in expected)
        {
            actual.Add(entry.Key, () => _categoryRepository.Add(entry.Value));
        }

        // Assert
        foreach (Action action in actual.Values)
        {
            Assert.Throws<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "Add: Add entities to repository")]
    public void Add_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];

        // Act
        foreach (Category category in expected.Values)
        {
            _categoryRepository.Add(category);
        }

        // Assert
        foreach (Category category in expected.Values)
        {
            Category actual = DbContext.Categories.Single(c => c.Id == category.Id);
            actual.Should().BeEquivalentTo(category);
        }
    }
}
