using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void UpdateRange_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.UpdateRange([ null! ]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.UpdateRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void UpdateRange_UpdateEntities_EntitiesChange()
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

        foreach (var product in expected)
        {
            product.Url = Fixture.Create<string>();
            product.Name = Fixture.Create<string>();
            product.MinOrder = Fixture.Create<int>();
        }

        // Act
        _productRepository.UpdateRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Products.Should().BeEquivalentTo(expected);
    }
}
