using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryGetByIdTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryGetByIdTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetById: Null Argument")]
    public void GetById_NullArgument_ReturnsNull()
    {
        // Act
        Category actual = _categoryRepository.GetById(null!);

        // Assert
        Assert.Null(actual);
    }

    [Fact(DisplayName = "GetById: Get entity by id")]
    public void GetById_GetEntityById_ReturnsEntity()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();

        Category expectedCategory = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

        // Act
        Category actual = _categoryRepository.GetById(expectedCategory.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }
}
