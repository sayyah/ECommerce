using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductDeleteRangeAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductDeleteRangeAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null product")]
    public void DeleteRangeAsync_NullProduct_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        Task actual() => _productRepository.DeleteRangeAsync(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null input")]
    public void DeleteRangeAsync_NullInput_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        Task actual() => _productRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete range of products from repository")]
    public async void DeleteRangeAsync_DeleteProducts_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        await _productRepository.DeleteRangeAsync(productsToDelete, CancellationToken);

        // Assert
        List<Product?> actual = new();
        foreach (var product in productsToDelete)
        {
            actual.Add(DbContext.Products.FirstOrDefault(x => x.Id == product.Id));
        }

        Assert.Equal(1, DbContext.Products.Count());
        foreach (var product in actual)
        {
            Assert.Null(product);
        }
    }

    [Fact(
        DisplayName = "DeleteRangeAsync: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public async void DeleteRangeAsync_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = expected.Keys.ToArray()[
            Random.Shared.Next(expected.Count)
        ];
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        await _productRepository.DeleteRangeAsync(productsToDelete, CancellationToken, false);

        // Assert
        List<Product?> actual = new();
        foreach (var product in productsToDelete)
        {
            actual.Add(DbContext.Products.FirstOrDefault(x => x.Id == product.Id));
        }

        Assert.Equal(expected.Count, DbContext.Products.Count());
        foreach (var product in actual)
        {
            Assert.NotNull(product);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (var product in productsToDelete)
        {
            actual.Add(DbContext.Products.FirstOrDefault(x => x.Id == product.Id));
        }

        Assert.Equal(1, DbContext.Products.Count());
        foreach (var product in actual)
        {
            Assert.Null(product);
        }
    }
}
