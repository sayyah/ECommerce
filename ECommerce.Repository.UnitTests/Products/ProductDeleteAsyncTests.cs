using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "DeleteAsync: Null product")]
    public async Task DeleteAsync_NullProduct_ThrowsException()
    {
        // Act
        Task Action() => _productRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete product from repository")]
    public async void DeleteAsync_DeleteProduct_EntityNotInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected["test_1"];

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
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected["test_1"];

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
