using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductDeleteAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductDeleteAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "DeleteAsync: Null product")]
    public void DeleteAsync_NullProduct_ThrowsException()
    {
        // Act
        Task action() => _productRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete product from repository")]
    public async void DeleteAsync_DeleteProduct_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected.Values.ToArray()[0];

        // Act
        await _productRepository.DeleteAsync(productToDelete, CancellationToken);

        // Assert
        Product? actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.Products.Count());
    }

    [Fact(
        DisplayName = "DeleteAsync: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public async void DeleteAsync_NoSave_EntityIsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected.Values.ToArray()[0];

        // Act
        await _productRepository.DeleteAsync(productToDelete, CancellationToken, false);

        // Assert
        Product? actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected.Count, DbContext.Products.Count());

        DbContext.SaveChanges();
        actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);
        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.Products.Count());
    }
}
