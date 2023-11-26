using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductAddAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductAddAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "AddAsync: Null value for required Fields")]
    public void AddAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        Dictionary<string, Func<Task<Product>>> actual = new();
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
            Assert.ThrowsAsync<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "AddAsync: Null product")]
    public void AddAsync_NullProduct_ThrowsException()
    {
        // Act
        Task action() => _productRepository.AddAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "AddAsync: Add product async")]
    public void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        Dictionary<string, Product> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(
                entry.Key,
                _productRepository.AddAsync(entry.Value, CancellationToken).Result
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
