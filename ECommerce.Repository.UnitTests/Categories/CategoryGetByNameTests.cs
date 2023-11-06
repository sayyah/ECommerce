using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryGetByNameTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryGetByNameTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetByName: Null Argument")]
    public void GetByName_NullArgument_ReturnsNull()
    {
        // Act
        Task<Category> actual() => _categoryRepository.GetByName(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(actual);
    }

    [Fact(DisplayName = "GetByName: Get entity by name")]
    public async void GetByName_GetEntityByName_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

        // Act
        Category actual = await _categoryRepository.GetByName(
            expectedCategory.Name,
            CancellationToken,
            expectedCategory.ParentId
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }

    // TODO: non existing name
}
