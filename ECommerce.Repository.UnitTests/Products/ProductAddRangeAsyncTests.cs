using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public async Task AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
       void Actual() => _productRepository.AddRange(expected.Values, CancellationToken);

        // Assert
        await Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null product")]
    public async Task AddRangeAsync_NullProduct_ThrowsException()
    {
        // Act
       void Actual() => _productRepository.AddRange([ null! ], CancellationToken);

        // Assert
        await Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null Argument")]
    public async Task AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
       void Actual() => _productRepository.AddRange(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add products all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        await _productRepository.AddRange(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, Product?> actual =  [ ];
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
        AddCategories();
        Dictionary<string, Product> expected = _testSets["unique_url"];

        // Act
        await _productRepository.AddRange(expected.Values, CancellationToken, false);

        // Assert
        Dictionary<string, Product?> actual =  [ ];
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
