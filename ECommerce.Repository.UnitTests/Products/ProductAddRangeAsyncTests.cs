using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductAddRangeAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductAddRangeAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public void AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        Task actual() => _productRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null product")]
    public void AddRangeAsync_NullProduct_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.AddRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null Argument")]
    public void AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add products all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        await _productRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.FirstOrDefault(x => x.Id == entry.Value.Id));
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRangeAsync: No save")]
    public async void AddRangeAsync_NoSave_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        await _productRepository.AddRangeAsync(expected.Values, CancellationToken, false);

        // Assert
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.FirstOrDefault(x => x.Id == entry.Value.Id));
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.FirstOrDefault(x => x.Id == entry.Value.Id));
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
