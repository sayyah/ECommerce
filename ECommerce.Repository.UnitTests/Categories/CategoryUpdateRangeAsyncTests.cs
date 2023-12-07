using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public async Task UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task Action() => _categoryRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update entities in repository")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
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
        await _categoryRepository.UpdateRangeAsync(categoriesToUpdate, CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}
