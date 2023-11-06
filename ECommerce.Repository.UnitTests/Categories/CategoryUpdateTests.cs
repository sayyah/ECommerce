using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public class CategoryUpdateTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUpdateTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }

    [Fact(DisplayName = "Update: Null Argument")]
    public void Update_NullArgument_ThrowsException()
    {
        // Act
        void action() => _categoryRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Update: Update entity in repository")]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Category> expected = CategoryTestsUtils.GetTestSets()["simple_tests"];
        DbContext.Categories.AddRange(expected.Values);
        DbContext.SaveChanges();
        Category categoryToUpdate = expected.Values.ToArray()[Random.Shared.Next(expected.Count)];

        categoryToUpdate.Name = Guid.NewGuid().ToString();

        // Act
        _categoryRepository.Update(categoryToUpdate);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected.Values);
    }
}
