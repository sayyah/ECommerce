using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "UpdateRange: Null Argument")]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "UpdateRange: Update entities in repository")]
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
