using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "DeleteRangeAsync: Null Argument")]
    public async Task DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete entities from repository")]
    public async void DeleteRangeAsync_DeleteEntities_EntitiesRemovedFromRepository()
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
        await _categoryRepository.DeleteRangeAsync(categoriesToRemove, CancellationToken);

        // Assert
        Assert.Equal(1, DbContext.Categories.Count());
        DbContext.Categories.Should().ContainEquivalentOf(expected.Values.ToArray()[randomIndex]);
    }
}
