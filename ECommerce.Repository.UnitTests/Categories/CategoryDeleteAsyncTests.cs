using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "DeleteAsync: Null Argument")]
    public async Task DeleteAsync_NullArgument_ThrowsException()
    {
        // Act
        Task Action() => _categoryRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete entity from repository")]
    public async void DeleteAsync_DeleteEntity_EntityRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToRemove = expected["test_1"];

        // Act
        await _categoryRepository.DeleteAsync(categoryToRemove, CancellationToken);

        // Assert
        Assert.Equal(expected.Count - 1, DbContext.Categories.Count());
        Exception exception = Assert.Throws<InvalidOperationException>(
            () => DbContext.Categories.Single(c => c.Id == categoryToRemove.Id)
        );
        Assert.Equal("Sequence contains no elements", exception.Message);
        var withoutDeletedCategory = expected.Values.Where(c => c.Id != categoryToRemove.Id);
        DbContext.Categories.Should().BeEquivalentTo(withoutDeletedCategory);
    }
}
