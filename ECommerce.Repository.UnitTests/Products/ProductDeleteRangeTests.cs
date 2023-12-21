using System.Data;
using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void DeleteRange_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.DeleteRange([ null! ]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void DeleteRange_DeleteProducts_EntityNotInRepository()
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

        Product productNotToDelete = products.ElementAt(2);
        IEnumerable<Product> productsToDelete = products.Where(x => x.Id != productNotToDelete.Id);

        // Act
        _productRepository.DeleteRange(productsToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Products.Count().Should().Be(1);
        DbContext
            .Products
            .Include(p => p.Store)
            .Include(p => p.HolooCompany)
            .FirstOrDefault()
            .Should()
            .BeEquivalentTo(productNotToDelete);
    }
}
