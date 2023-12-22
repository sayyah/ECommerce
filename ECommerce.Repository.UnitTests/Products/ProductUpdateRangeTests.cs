using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void UpdateRange_NullProduct_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.UpdateRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _productRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        AddCategories();
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
        Dictionary<string, Product?> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
