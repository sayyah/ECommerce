using System.Data;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "DeleteRange: Null Product")]
    public void DeleteRange_NullProduct_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.DeleteRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact(DisplayName = "DeleteRange: Null Argument")]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact(DisplayName = "DeleteRange: Delete range of products from repository")]
    public void DeleteRange_DeleteProducts_EntityNotInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = "test_1";
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        _productRepository.DeleteRange(productsToDelete);

        // Assert
        List<Product?> actual =  [ ];
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
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        string productNotToDeleteSetKey = "test_1";
        Product productNotToDelete = expected[productNotToDeleteSetKey];
        IEnumerable<Product> productsToDelete = expected
            .Values
            .Where(x => x.Id != productNotToDelete.Id);

        // Act
        _productRepository.DeleteRange(productsToDelete, false);

        // Assert
        List<Product?> actual =  [ ];
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
