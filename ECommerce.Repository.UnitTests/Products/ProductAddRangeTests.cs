using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductAddRangeTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductAddRangeTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "AddRange: Null value for required Fields")]
    public void AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        void actual() => _productRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null product")]
    public void AddRange_NullProduct_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        void actual() => _productRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRange: Add products all together")]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        _productRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, Product?> actual = new();
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.FirstOrDefault(x => x.Id == entry.Value.Id));
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRange: No save")]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        _productRepository.AddRange(expected.Values, false);

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
