using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryUpdateRangeAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUpdateRangeAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public void UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update entities in repository")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
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
