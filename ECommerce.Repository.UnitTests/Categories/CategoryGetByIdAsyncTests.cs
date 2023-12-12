using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "GetByIdAsync: Null Argument")]
    public async void GetByIdAsync_NullArgument_ReturnsNull()
    {
        // Act
        Category actual = await _categoryRepository.GetByIdAsync(CancellationToken, null!);

        // Assert
        Assert.Null(actual);
    }

    [Fact(DisplayName = "GetByIdAsync: Get entity by id")]
    public async void GetByIdAsync_GetEntityById_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected["test_2"];

        // Act
        Category actual = await _categoryRepository.GetByIdAsync(
            CancellationToken,
            expectedCategory.Id
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }
}
