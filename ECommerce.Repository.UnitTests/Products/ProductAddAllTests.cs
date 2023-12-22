using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void AddAll_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        void Actual() => _productRepository.AddAll(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact]
    public void AddAll_NullProduct_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.AddAll([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void AddAll_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.AddAll(null!);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void AddAll_AddEntities_ReturnsAddedEntitiesCount()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        _productRepository.AddAll(expected.Values);

        // Assert
        Assert.Equal(expected.Count, DbContext.Products.Count());
    }
}
