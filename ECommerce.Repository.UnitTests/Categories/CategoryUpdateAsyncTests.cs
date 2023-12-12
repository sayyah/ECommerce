using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "UpdateAsync: Null Argument")]
    public async Task UpdateAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "UpdateAsync: Update entity in repository")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToUpdate = expected["test_2"];

        categoryToUpdate.Name = Guid.NewGuid().ToString();

        // Act
        await _categoryRepository.UpdateAsync(categoryToUpdate, CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}
