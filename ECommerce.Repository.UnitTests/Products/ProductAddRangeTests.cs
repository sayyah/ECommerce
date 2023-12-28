using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void AddRange_RequiredNameField_ThrowsException()
    {
        // Arrange
        var product = Fixture.Create<Product>();
        product.Name = null!;
        var products = new List<Product> { product };

        // Act
        async Task Action()
        {
            _productRepository.AddRange(products);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredUrlField_ThrowsException()
    {
        // Arrange
        var product = Fixture.Create<Product>();
        product.Name = null!;
        var products = new List<Product> { product };

        // Act
        async Task Action()
        {
            _productRepository.AddRange(products);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_NullProduct_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.AddRange(new List<Product> { null! });
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void AddRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _productRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.CreateMany<Product>(2).ToList();

        // Act
        _productRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products;
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
