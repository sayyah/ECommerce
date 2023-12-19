using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        var set = _testSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        var expected = set["test_2"];

        // Act
        var actual = await _productRepository.GetByName(expected.Name, CancellationToken);

        // Assert
        actual
            .Should()
            .BeEquivalentTo(
                expected,
                options =>
                    options
                        .For(x => x.ProductCategories)
                        .Exclude(x => x.Products)
                        .For(x => x.Prices)
                        .Exclude(x => x.Product)
            );
    }

    [Fact]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        AddCategories();
        var set = _testSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        // Act
        var actual = await _productRepository.GetByName(new Guid().ToString(), CancellationToken);

        // Assert
        actual.Should().BeNull();
    }
}
