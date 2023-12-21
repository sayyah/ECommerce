using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void GetByUrl_GetAddedEntityByUrl_EntityExistsInRepository()
    {
        // Arrange
        var price = Fixture
            .Build<Price>()
            .With(p => p.ProductId, 1)
            .Without(p => p.Product)
            .Without(p => p.Unit)
            .Without(p => p.UnitId)
            .Without(p => p.Size)
            .Without(p => p.SizeId)
            .Without(p => p.Color)
            .Without(p => p.ColorId)
            .Without(p => p.Currency)
            .Without(p => p.CurrencyId)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();
        var expected = Fixture
            .Build<Product>()
            .With(p => p.Id, 1)
            .With(p => p.Prices, [ price ])
            .Without(p => p.ProductCategories)
            .Without(p => p.ProductComments)
            .Without(p => p.ProductUserRanks)
            .Without(p => p.AttributeGroupProducts)
            .Without(p => p.AttributeValues)
            .Without(p => p.Images)
            .Without(p => p.Supplier)
            .Without(p => p.SupplierId)
            .Without(p => p.Brand)
            .Without(p => p.BrandId)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.SlideShows)
            .Create();
        IEnumerable<Product> products =
        [
            ..Fixture
            .Build<Product>()
            .Without(p => p.Prices)
            .Without(p => p.ProductCategories)
            .Without(p => p.ProductComments)
            .Without(p => p.ProductUserRanks)
            .Without(p => p.AttributeGroupProducts)
            .Without(p => p.AttributeValues)
            .Without(p => p.Images)
            .Without(p => p.Supplier)
            .Without(p => p.SupplierId)
            .Without(p => p.Brand)
            .Without(p => p.BrandId)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.SlideShows)
            .CreateMany(3),
            expected,
            ..Fixture
            .Build<Product>()
            .Without(p => p.Prices)
            .Without(p => p.ProductCategories)
            .Without(p => p.ProductComments)
            .Without(p => p.ProductUserRanks)
            .Without(p => p.AttributeGroupProducts)
            .Without(p => p.AttributeValues)
            .Without(p => p.Images)
            .Without(p => p.Supplier)
            .Without(p => p.SupplierId)
            .Without(p => p.Brand)
            .Without(p => p.BrandId)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.SlideShows)
            .CreateMany(5),
        ];
        DbContext.Products.AddRange(products);
        DbContext.SaveChanges();

        // Act
        var actual = await _productRepository.GetByUrl(expected.Url, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetByUrl_GetAddedEntityByNonExistingUrl_ReturnsNull()
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

        // Act
        var actual = await _productRepository.GetByUrl(new Guid().ToString(), CancellationToken);

        // Assert
        actual.Should().BeNull();
    }
}
