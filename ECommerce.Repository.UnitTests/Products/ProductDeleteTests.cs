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
        var products = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);
        Product productToDelete = products.ElementAt(1);

        // Act
        _productRepository.Delete(productToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        Product? actual = DbContext.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        // Assert
        actual.Should().BeNull();
        DbContext.Products.Count().Should().Be(products.Count() - 1);
    }
}
