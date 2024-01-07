using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
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
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
        Dictionary<string, Action> actual =  [ ];
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
        Category expected = TestSets["simple_tests"]["test_1"];

        // Act
        _categoryRepository.Add(expected);

        // Assert
        Category actual = DbContext.Categories.Single(c => c.Id == expected.Id);
        actual.Should().BeEquivalentTo(expected);
    }
}
