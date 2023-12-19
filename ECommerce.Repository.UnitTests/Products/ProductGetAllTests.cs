using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        var actuals = _productRepository.GetAll("");

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(p => p.Prices).Excluding(p => p.ProductCategories)
            );
    }
}
