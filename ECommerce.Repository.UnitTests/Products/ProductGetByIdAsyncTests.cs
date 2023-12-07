using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "GetByIdAsync: Get products by Id")]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        var set = _testSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        Product expected = set["test_2"];

        // Act
        var actual = await _productRepository.GetByIdAsync(CancellationToken, expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
