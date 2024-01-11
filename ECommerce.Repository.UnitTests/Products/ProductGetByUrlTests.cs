using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async Task GetByUrl_GetAEntityByUrl_ReturnEntity()
    {
        // Arrange
        var expected = Fixture.Create<Product>();
        var products = new List<Product>() { expected };
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);

        // Act
        var actual = await _productRepository.GetByUrl(expected.Url, CancellationToken);

        // Assert
        actual?.Id.Should().Be(expected.Id);
    }

    [Fact]
    public async Task GetByUrl_GetAddedEntityByNonExistingUrl_ReturnsNull()
    {
        // Arrange
        var products = Fixture.CreateMany<Product>(1);
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);

        // Act
        var actual = await _productRepository.GetByUrl(new Guid().ToString(), CancellationToken);

        // Assert
        actual.Should().BeNull();
    }
}
