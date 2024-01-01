using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void GetAll_GetAllEntities_ReturnsAllEntitiesInRepository()
    {
        // Arrange
        var parent = Fixture
            .Build<Category>()
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();
        var children = Fixture
            .Build<Category>()
            .With(p => p.Parent, () => parent)
            .With(p => p.ParentId, () => parent.Id)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .CreateMany(5);
        parent.Categories = children.ToList();
        var expected = children.Append(parent);
        DbContext.Categories.AddRange(expected);
        DbContext.SaveChanges();

        // Act
        IEnumerable<Category> actuals = await _categoryRepository.GetAllAsync(CancellationToken);

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected,
                options => options.Excluding(c => c.Categories).Excluding(c => c.Parent)
            );
    }
}
