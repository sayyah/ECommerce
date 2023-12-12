using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "UpdateAsync: Null input")]
    public async Task UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<Product> actual() => _productRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update product")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToUpdate = expected["test_2"];
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
