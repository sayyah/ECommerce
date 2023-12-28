using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
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
        var products = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);

        Product expectedProduct = products.ElementAt(1);
        expectedProduct.Url = Guid.NewGuid().ToString();
        expectedProduct.Name = Guid.NewGuid().ToString();
        expectedProduct.MinOrder = Random.Shared.Next();

        // Act
        _productRepository.Update(expectedProduct);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products.Single(p => p.Id == expectedProduct.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedProduct);
    }
}
