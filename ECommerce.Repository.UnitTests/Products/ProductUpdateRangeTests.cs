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
            _productRepository.UpdateRange(new List<Product> { null! });
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
        var expected = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(expected);
        await DbContext.SaveChangesAsync();
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
