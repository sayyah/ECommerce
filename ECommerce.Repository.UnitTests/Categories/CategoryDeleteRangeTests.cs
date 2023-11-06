using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryDeleteRangeTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDeleteRangeTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRange: Null Argument")]
    public void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.DeleteRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteRange: Delete entities from repository")]
    public void DeleteRange_DeleteEntities_EntitiesRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
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
