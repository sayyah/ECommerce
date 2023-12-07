using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "DeleteRange: Null Argument")]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "DeleteRange: Delete entities from repository")]
    public void DeleteRange_DeleteEntities_EntitiesRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        var randomIndex = Random.Shared.Next(expected.Count);
        IEnumerable<Category> categoriesToRemove = expected
            .Values
            .ToArray()
            .Where((_, index) => index != randomIndex);

        // Act
        _categoryRepository.DeleteRange(categoriesToRemove);

        // Assert
        Assert.Equal(1, DbContext.Categories.Count());
        DbContext.Categories.Should().ContainEquivalentOf(expected.Values.ToArray()[randomIndex]);
    }
}
