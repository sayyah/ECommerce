using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "GetByUrl: Get products by url")]
    public async void GetByUrl_GetAddedEntityByUrl_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        var set = TestSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        var expected = set["test_2"];

        // Act
        var actual = await _productRepository.GetByUrl(expected.Url, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact(DisplayName = "GetByUrl: Non existing url")]
    public async void GetByUrl_GetAddedEntityByNonExistingUrl_ReturnsNull()
    {
        // Arrange
        AddCategories();
        var set = TestSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        // Act
        var actual = await _productRepository.GetByUrl(new Guid().ToString(), CancellationToken);

        // Assert
        actual.Should().BeNull();
    }
}
