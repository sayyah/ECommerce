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

        Product expected = products.ElementAt(2);

        // Act
        var actual = await _productRepository.GetByIdAsync(CancellationToken, expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
