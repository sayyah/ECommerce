using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductUpdateAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductUpdateAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "UpdateAsync: Null input")]
    public void UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<Product> actual() => _productRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update product")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
        Product expectedProduct = DbContext.Products.Single(p => p.Id == productToUpdate.Id)!;
        expectedProduct.Url = Guid.NewGuid().ToString();
        expectedProduct.Name = Guid.NewGuid().ToString();
        expectedProduct.MinOrder = Random.Shared.Next();

        // Act
        await _productRepository.UpdateAsync(expectedProduct, CancellationToken);

        // Assert
        Product? actual = DbContext.Products.Single(p => p.Id == productToUpdate.Id);
        Assert.Equivalent(expectedProduct, actual);
    }
}
