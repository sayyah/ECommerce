using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "UpdateRangeAsync: Null Product")]
    public async Task UpdateRangeAsync_NullProduct_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public async Task UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _productRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update products")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];
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
        Dictionary<string, Product?> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
