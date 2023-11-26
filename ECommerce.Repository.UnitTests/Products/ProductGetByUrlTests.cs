using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductGetByUrlTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductGetByUrlTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "GetByUrl: Get products by url")]
    public async void GetByUrl_GetAddedEntityByUrl_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        foreach (var product in expected.Values)
        {
            DbContext.Products.Add(product);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _productRepository.GetByUrl(entry.Value.Url, CancellationToken)
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "GetByUrl: Non existing url")]
    public async void GetByUrl_GetAddedEntityByNonExistingUrl_ReturnsNull()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        foreach (var product in expected.Values)
        {
            DbContext.Products.Add(product);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, Product?> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _productRepository.GetByUrl(new Guid().ToString(), CancellationToken)
            );
        }

        // Assert
        actual.Values.Should().AllSatisfy(x => x.Should().BeNull());
    }
}
