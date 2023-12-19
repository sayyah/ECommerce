using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void Delete_NullProduct_ThrowsException()
    {
        // Act
        void Action() => _productRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Delete_DeleteProduct_EntityNotInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected["test_1"];

        // Act
        _productRepository.Delete(productToDelete);

        // Assert
        Product? actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.Products.Count());
    }

    [Fact(
        DisplayName = "Delete: (No Save) Entity is in repository and is deleted after SaveChanges is called"
    )]
    public void Delete_NoSave_EntityIsInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = expected["test_1"];

        // Act
        _productRepository.Delete(productToDelete);

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
