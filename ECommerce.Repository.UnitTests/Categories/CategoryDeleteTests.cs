using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public void Delete_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Delete_DeleteEntity_EntityRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToRemove = expected["test_1"];

        // Act
        _categoryRepository.Delete(categoryToRemove);

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
