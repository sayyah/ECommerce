using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryUpdateAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUpdateAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateAsync: Null Argument")]
    public void UpdateAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "UpdateAsync: Update entity in repository")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToUpdate = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

        categoryToUpdate.Name = Guid.NewGuid().ToString();

        // Act
        await _categoryRepository.UpdateAsync(categoryToUpdate, CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}
