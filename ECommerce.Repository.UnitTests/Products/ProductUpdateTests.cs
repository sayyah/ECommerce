using AutoFixture;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void Update_NullInput_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.Update(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        var expected = Fixture
            .Build<Product>()
            .Without(p => p.ProductCategories)
            .Without(p => p.ProductComments)
            .Without(p => p.ProductUserRanks)
            .Without(p => p.AttributeGroupProducts)
            .Without(p => p.AttributeValues)
            .Without(p => p.Prices)
            .Without(p => p.Images)
            .Without(p => p.Supplier)
            .Without(p => p.SupplierId)
            .Without(p => p.Brand)
            .Without(p => p.BrandId)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.SlideShows)
            .CreateMany(5);
        DbContext.Products.AddRange(expected);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToUpdate = expected.ElementAt(2);
        Product expectedProduct = DbContext.Products.Single(p => p.Id == productToUpdate.Id)!;
        expectedProduct.Url = Guid.NewGuid().ToString();
        expectedProduct.Name = Guid.NewGuid().ToString();
        expectedProduct.MinOrder = Random.Shared.Next();

        // Act
        _productRepository.Update(expectedProduct);
        await UnitOfWork.SaveAsync(CancellationToken);
        Product? actual = DbContext.Products.Single(p => p.Id == productToUpdate.Id);

        // Assert
        Assert.Equivalent(expectedProduct, actual);
    }
}
