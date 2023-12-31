using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact(DisplayName = "Update: Null Argument")]
    public void Update_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Update: Update entity in repository")]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Category> expected = TestSets["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToUpdate = expected["test_3"];

        categoryToUpdate.Name = Guid.NewGuid().ToString();

        // Act
        _categoryRepository.Update(categoryToUpdate);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}