using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        var products = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);
        Product expected = products.ElementAt(1);

        // Act
        var actual = await _productRepository.GetByIdAsync(CancellationToken, expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
