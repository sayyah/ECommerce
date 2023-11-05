using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductAddAllTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductAddAllTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "AddAll: Null value for required Fields")]
    public void AddAll_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        Task<int> actual() => _productRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddAll: Null product")]
    public void AddAll_NullProduct_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        Task<int> actual() => _productRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddAll: Add products all together")]
    public async void AddAll_AddEntities_ReturnsAddedEntitiesCount()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        await _productRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.Equal(expected.Count, DbContext.Products.Count());
        Assert.NotEqual(0, DbContext.Prices.Count());
        Assert.NotEqual(0, DbContext.Prices.Count());
    }
}
