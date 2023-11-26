using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductUpdateRangeAsyncTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductUpdateRangeAsyncTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Product")]
    public void UpdateRangeAsync_NullProduct_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public void UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update products")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, Product> entry in expected)
        {
            expected[entry.Key] = DbContext.Products.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].Url = Guid.NewGuid().ToString();
            expected[entry.Key].Name = Guid.NewGuid().ToString();
            expected[entry.Key].MinOrder = Random.Shared.Next();
        }

        // Act
        await _productRepository.UpdateRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
