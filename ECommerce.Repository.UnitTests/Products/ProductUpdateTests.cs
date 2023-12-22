﻿using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToUpdate = expected["test_2"];
        Product expectedProduct = DbContext.Products.Single(p => p.Id == productToUpdate.Id)!;
        expectedProduct.Url = Guid.NewGuid().ToString();
        expectedProduct.Name = Guid.NewGuid().ToString();
        expectedProduct.MinOrder = Random.Shared.Next();

        // Act
        _productRepository.Update(expectedProduct);

        // Assert
        Product? actual = DbContext.Products.Single(p => p.Id == productToUpdate.Id);
        Assert.Equivalent(expectedProduct, actual);
    }
}
