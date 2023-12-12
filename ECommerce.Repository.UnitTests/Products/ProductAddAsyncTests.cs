﻿using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "AddAsync: Null value for required Fields")]
    public async Task AddAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = TestSets["required_fields"];

        // Act
        Dictionary<string, Func<Task<Product>>> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(
                entry.Key,
                () => _productRepository.AddAsync(entry.Value, CancellationToken)
            );
        }

        // Assert
        foreach (var action in actual.Values)
        {
            await Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "AddAsync: Null product")]
    public async Task AddAsync_NullProduct_ThrowsException()
    {
        // Act
        Task action() => _productRepository.AddAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAsync: Add product async")]
    public async void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        AddCategories();
        Product expected = TestSets["unique_url"]["test_1"];

        // Act
        var actual = await _productRepository.AddAsync(expected, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
