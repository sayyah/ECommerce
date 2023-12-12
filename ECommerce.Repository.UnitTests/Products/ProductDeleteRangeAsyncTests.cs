using System.Data;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "DeleteRangeAsync: Null Product")]
    public async Task DeleteRangeAsync_NullProduct_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.DeleteRangeAsync([ null! ], CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null Argument")]
    public async Task DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete range of products from repository")]
    public async void DeleteRangeAsync_DeleteProducts_EntityNotInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];
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
        DisplayName = "DeleteRangeAsync: (No Save) Entites are in repository and are deleted after SaveChanges is called"
    )]
    public async void DeleteRangeAsync_NoSave_EntitiesAreInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];
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
