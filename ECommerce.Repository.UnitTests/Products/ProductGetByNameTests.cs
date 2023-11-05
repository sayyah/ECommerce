using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductGetByNameTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductGetByNameTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "GetByName: Get products by Name")]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
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
                await _productRepository.GetByName(entry.Value.Name, CancellationToken)
            );
        }

        // Assert
        actual
            .Values
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options =>
                    options
                        .For(x => x.ProductCategories)
                        .Exclude(x => x.Products)
                        .For(x => x.Prices)
                        .Exclude(x => x.Product)
            );
    }

    [Fact(DisplayName = "GetByName: Non existing name")]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
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
                await _productRepository.GetByName(new Guid().ToString(), CancellationToken)
            );
        }

        // Assert
        actual.Values.Should().AllSatisfy(x => x.Should().BeNull());
    }

    [Fact(DisplayName = "GetByName: Non existing url")]
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
