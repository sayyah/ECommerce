﻿using AutoFixture;
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
        var list = Fixture
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
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _productRepository.AddRange(list);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredUrlField_ThrowsException()
    {
        // Arrange
        var list = Fixture
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
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _productRepository.AddRange(list);
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
            _productRepository.AddRange([ null! ]);
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

        // Act
        _productRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Products;
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
