using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductDeleteRangeTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductDeleteRangeTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "DeleteRange: Null product")]
    public void DeleteRange_NullProduct_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        void actual() => _productRepository.DeleteRange(expected.Values);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Null input")]
    public void DeleteRange_NullInput_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        void actual() => _productRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRange: Delete range of products from repository")]
    public void DeleteRange_DeleteProducts_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = expected.Keys.ToArray()[0];
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        _productRepository.DeleteRange(productsToDelete);

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
        DisplayName = "DeleteRange: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public void DeleteRange_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = expected.Keys.ToArray()[0];
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        _productRepository.DeleteRange(productsToDelete, false);

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
