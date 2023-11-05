using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductGetAllTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductGetAllTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "GetAll: Get all products")]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        foreach (var product in expected.Values)
        {
            DbContext.Products.Add(product);
        }
        DbContext.SaveChanges();

        // Act
        var actuals = _productRepository.GetAll(CancellationToken).Result;

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(p => p.Prices).Excluding(p => p.ProductCategories)
            );
    }
}
