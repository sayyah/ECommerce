using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async Task AddAsync_RequiredNameField_ThrowsException()
    {
        // Arrange
        var product = Fixture
            .Build<Product>()
            .With(p => p.Name, () => null!)
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
            .Create();

        // Act
        async Task Action()
        {
            await _productRepository.AddAsync(product, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task AddAsync_RequiredUrlField_ThrowsException()
    {
        // Arrange
        var product = Fixture
            .Build<Product>()
            .With(p => p.Url, () => null!)
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
            .Create();

        // Act
        async Task Action()
        {
            await _productRepository.AddAsync(product, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task AddAsync_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _productRepository.AddAsync(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        Product expected = Fixture
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
            .Create();

        // Act
        await _productRepository.AddAsync(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products.Single(p => p.Id == expected.Id);

        // Assert
        DbContext.Products.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
