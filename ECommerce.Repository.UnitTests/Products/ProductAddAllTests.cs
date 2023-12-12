using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "AddAll: Null value for required Fields")]
    public async Task AddAll_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = TestSets["required_fields"];

        // Act
        Task<int> actual() => _productRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddAll: Null product")]
    public async Task AddAll_NullProduct_ThrowsException()
    {
        // Act
        Task<int> actual() => _productRepository.AddAll([ null! ], CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddAll: Null Argument")]
    public async Task AddAll_NullArgument_ThrowsException()
    {
        // Act
        Task<int> actual() => _productRepository.AddAll(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddAll: Add products all together")]
    public async void AddAll_AddEntities_ReturnsAddedEntitiesCount()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];

        // Act
        await _productRepository.AddAll(expected.Values, CancellationToken);

        // Assert
        Assert.Equal(expected.Count, DbContext.Products.Count());
    }
}
