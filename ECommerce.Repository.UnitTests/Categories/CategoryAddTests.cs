using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public void Add_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
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

    [Fact]
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
