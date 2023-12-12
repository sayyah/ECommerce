using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "GetAll: Get all entities in repository")]
    public async void GetAll_GetAllEntities_ReturnsAllEntitiesInRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        IEnumerable<Category> actuals = await _categoryRepository.GetAll(CancellationToken);

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(c => c.Categories).Excluding(c => c.Parent)
            );
    }
}
