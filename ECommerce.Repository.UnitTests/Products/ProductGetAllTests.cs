using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "GetAll: Get all products")]
    public async void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        var actuals = await _productRepository.GetAll(CancellationToken);

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(p => p.Prices).Excluding(p => p.ProductCategories)
            );
    }
}
