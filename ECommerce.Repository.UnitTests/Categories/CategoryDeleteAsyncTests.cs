using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryDeleteAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDeleteAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "DeleteAsync: Null Argument")]
    public void DeleteAsync_NullArgument_ThrowsException()
    {
        // Act
        Task action() => _categoryRepository.DeleteAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "DeleteAsync: Delete entity from repository")]
    public async void DeleteAsync_DeleteEntity_EntityRemovedFromRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToRemove = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

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
