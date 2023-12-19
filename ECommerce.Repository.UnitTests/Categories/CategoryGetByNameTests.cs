using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void GetByName_GetEntityByName_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected["test_3"];

        // Act
        Category? actual = await _categoryRepository.GetByName(
            expectedCategory.Name,
            CancellationToken,
            expectedCategory.ParentId
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }

    // TODO: non existing name
}
