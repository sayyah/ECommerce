using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        var randomIndex = Random.Shared.Next(expected.Count);
        IEnumerable<Category> categoriesToUpdate = expected
            .Values
            .ToArray()
            .Where((_, index) => index != randomIndex);

        foreach (var item in categoriesToUpdate)
        {
            item.Name = Guid.NewGuid().ToString();
        }

        // Act
        _categoryRepository.UpdateRange(categoriesToUpdate);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}
