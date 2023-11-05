using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductUpdateRangeTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductUpdateRangeTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "UpdateRange: Null input")]
    public void UpdateRange_NullInput_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        void actual() => _productRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRange: Update products")]
    public void UpdateRange_UpdateEntities_EntitiesChange()
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
        _productRepository.UpdateRange(expected.Values);

        // Assert
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
