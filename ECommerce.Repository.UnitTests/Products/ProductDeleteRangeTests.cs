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
            _productRepository.DeleteRange(new List<Product>() { null! });
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
    public async Task DeleteRange_DeleteProducts_EntityNotInRepository()
    {
        // Arrange
        var product1 = Fixture.Create<Product>();
        var product2 = Fixture.Create<Product>();
        var products = new List<Product>() { product1, product2 };
        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync(CancellationToken);
        var expected = product1;
        var productsToDelete = new List<Product>() { product2 };

        // Act
        _productRepository.DeleteRange(productsToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products.FirstOrDefault();

        // Assert
        DbContext.Products.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
