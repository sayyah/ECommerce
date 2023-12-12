using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "GetById: Get products by Id")]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        var set = TestSets["unique_url"];
        DbContext.Products.AddRange(set.Values);
        DbContext.SaveChanges();

        Product expected = set["test_2"];

        // Act
        var actual = _productRepository.GetById(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
