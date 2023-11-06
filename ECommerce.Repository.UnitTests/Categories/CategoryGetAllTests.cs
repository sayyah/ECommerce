using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryGetAllTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryGetAllTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetAll: Get all entities in repository")]
    public async void GetAll_GetAllEntities_ReturnsAllEntitiesInRepository()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        IEnumerable<Category> actuals = await _categoryRepository.GetAll(CancellationToken);

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(c => c.Categories).Excluding(c => c.Parent)
            );
    }
}
