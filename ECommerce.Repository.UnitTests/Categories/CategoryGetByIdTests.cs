using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "GetById: Null Argument")]
    public void GetById_NullArgument_ReturnsNull()
    {
        // Act
        Category actual = _categoryRepository.GetById(null!);

        // Assert
        Assert.Null(actual);
    }

    [Fact(DisplayName = "GetById: Get entity by id")]
    public void GetById_GetEntityById_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected["test_2"];

        // Act
        Category actual = _categoryRepository.GetById(expectedCategory.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }
}
