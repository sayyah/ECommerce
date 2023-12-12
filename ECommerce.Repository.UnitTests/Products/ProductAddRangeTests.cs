using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact(DisplayName = "AddRange: Null value for required Fields")]
    public void AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = TestSets["required_fields"];

        // Act
        void actual() => _productRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null product")]
    public void AddRange_NullProduct_ThrowsException()
    {
        // Act
        void actual() => _productRepository.AddRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null Argument")]
    public void AddRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _productRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRange: Add products all together")]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];

        // Act
        _productRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, Product?> actual =  [ ];
        foreach (KeyValuePair<string, Product> entry in expected)
        {
            actual.Add(entry.Key, DbContext.Products.FirstOrDefault(x => x.Id == entry.Value.Id));
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRange: No save")]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        AddCategories();
        Dictionary<string, Product> expected = TestSets["unique_url"];

        // Act
        _productRepository.AddRange(expected.Values, false);

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
