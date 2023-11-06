using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryGetByIdAsyncTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryGetByIdAsyncTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetByIdAsync: Null Argument")]
    public async void GetByIdAsync_NullArgument_ReturnsNull()
    {
        // Act
        Category actual = await _categoryRepository.GetByIdAsync(CancellationToken, null!);

        // Assert
        Assert.Null(actual);
    }

    [Fact(DisplayName = "GetByIdAsync: Get entity by id")]
    public async void GetByIdAsync_GetEntityById_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

        // Act
        Category actual = await _categoryRepository.GetByIdAsync(
            CancellationToken,
            expectedCategory.Id
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }
}
