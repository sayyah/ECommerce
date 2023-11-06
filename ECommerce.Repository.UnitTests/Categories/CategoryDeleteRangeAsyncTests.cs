using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryDeleteRangeAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDeleteRangeAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Null Argument")]
    public void DeleteRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.DeleteRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteRangeAsync: Delete entities from repository")]
    public async void DeleteRangeAsync_DeleteEntities_EntitiesRemovedFromRepository()
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
        await _categoryRepository.DeleteRangeAsync(categoriesToRemove, CancellationToken);

        // Assert
        Assert.Equal(1, DbContext.Categories.Count());
        DbContext.Categories.Should().ContainEquivalentOf(expected.Values.ToArray()[randomIndex]);
    }
}
