using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public void AddRange_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void AddRange_RequiredArguments_ThrowsException()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["required"];

        // Act
        void Actual() => _categoryRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact]
    public void AddRange_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];

        // Act
        _categoryRepository.AddRange(expected.Values);

        // Assert
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected.Values);
    }
}
