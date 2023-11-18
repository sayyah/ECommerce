using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void AddAll_RequiredNameField_ThrowsException()
    {
        // Arrange
        var product = Fixture.Create<Product>();
        product.Name = null!;
        var products = new List<Product> { product };

        // Act
        async Task Action()
        {
            await _productRepository.AddAll(products);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAll_RequiredUrlField_ThrowsException()
    {
        // Arrange
        var product = Fixture.Create<Product>();
        product.Url = null!;
        var products = new List<Product> { product };

        // Act
        async Task Action()
        {
            await _productRepository.AddAll(products);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAll_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _productRepository.AddAll(new List<Product>() { null! });
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void AddAll_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _productRepository.AddAll(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void AddAll_AddEntities_ReturnsAddedEntitiesCount()
    {
        // Arrange
        var expected = Fixture.CreateMany<Product>(2).ToList();

        // Act
        await _productRepository.AddAll(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products.ToList();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
