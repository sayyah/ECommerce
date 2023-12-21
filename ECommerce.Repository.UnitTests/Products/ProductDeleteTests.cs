using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void Delete_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Delete_DeleteProduct_EntityNotInRepository()
    {
        // Arrange
        var products = Fixture
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
        DbContext.Products.AddRange(products);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToDelete = products.ElementAt(2);

        // Act
        _productRepository.Delete(productToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        Product? actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        // Assert
        actual.Should().BeNull();
        DbContext.Products.Count().Should().Be(products.Count() - 1);
    }
}
