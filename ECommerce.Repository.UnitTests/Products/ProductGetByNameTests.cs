using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        var products = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);
        var expected = products.ElementAt(1);

        // Act
        var actual = await _productRepository.GetByName(expected.Name, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        var products = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);

        // Act
        var actual = await _productRepository.GetByName(new Guid().ToString(), CancellationToken);

        // Assert
        actual.Should().BeNull();
    }
}
