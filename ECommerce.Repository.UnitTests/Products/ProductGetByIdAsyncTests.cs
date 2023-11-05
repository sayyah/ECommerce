using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductGetByIdAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductGetByIdAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "GetByIdAsync: Get products by Id")]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        foreach (var product in expected.Values)
        {
            DbContext.Products.Add(product);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, Product> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _productRepository.GetByIdAsync(CancellationToken, entry.Value.Id)
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
