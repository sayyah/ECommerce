using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryDeleteTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDeleteTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "Delete: Null Argument")]
    public void Delete_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Delete: Delete entity from repository")]
    public void Delete_DeleteEntity_EntityRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToRemove = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

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
