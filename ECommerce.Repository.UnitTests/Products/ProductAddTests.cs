using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "Add: Null value for required Fields")]
    public void Add_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["required_fields"];

        // Act
        Dictionary<string, Action> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, () => _productRepository.Add(entry.Value));
        }

        // Assert
        foreach (var action in actual.Values)
        {
            Assert.Throws<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "Add: Null product")]
    public void Add_NullProduct_ThrowsException()
    {
        // Act
        void Action() => _productRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "Add: Add product")]
    public void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        Product expected = _testSets["unique_url"]["test_1"];

        // Act
        _productRepository.Add(expected);

        // Assert
        var actual = DbContext.Products.FirstOrDefault(x => x.Id == expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }
}
